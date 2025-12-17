using System.Data.Common;
using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Microsoft.Extensions.Logging;

namespace Adria.Infrastructure.Persistence.Queries;

public sealed class GetNextCourseModuleQuery 
    : IGetNextCourseModuleQuery
{
    private static readonly string sql_moduleWithWrongAnswers = @"
        SELECT DISTINCT m.ModuleID, m.Position, m.Content
        FROM Module m
        INNER JOIN QuestionAnswer qa ON m.ModuleID = qa.ModuleID
        WHERE m.CourseID = @courseId
          AND EXISTS (
              -- User has answered this question wrong
              SELECT 1 
              FROM UserQuestionAnswer uqa 
              WHERE uqa.QAID = qa.QAID 
                AND uqa.AdriaID = @adriaId
                AND uqa.IsCorrectlyAnswered = FALSE
          )
          AND NOT EXISTS (
              -- User has NEVER answered this question correctly
              SELECT 1
              FROM QuestionAnswer qa2
              INNER JOIN UserQuestionAnswer uqa2 ON qa2.QAID = uqa2.QAID
              WHERE qa2.QuestionID = qa.QuestionID
                AND uqa2.AdriaID = @adriaId
                AND uqa2.IsCorrectlyAnswered = TRUE
          )
        ORDER BY m.Position ASC
        LIMIT 1;
    ";
    
    private static readonly string sql_nextModule = @"
        SELECT m.ModuleID, m.Position, m.Content
        FROM Module m
        WHERE m.CourseID = @courseId
          AND NOT EXISTS (
              SELECT 1
              FROM QuestionAnswer qa
              INNER JOIN UserQuestionAnswer uqa ON qa.QAID = uqa.QAID
              WHERE qa.ModuleID = m.ModuleID
                AND uqa.AdriaID = @adriaId
          )
        ORDER BY m.Position ASC
        LIMIT 1;
    ";

    private static readonly string sql_wrongQuestions = @"
    SELECT DISTINCT 
        q.QuestionID,
        q.Question,
        a.AnswerID,
        a.Answer
    FROM QuestionAnswer qa
    INNER JOIN Question q ON qa.QuestionID = q.QuestionID
    INNER JOIN Answer a ON qa.AnswerID = a.AnswerID
    WHERE qa.ModuleID = @moduleId
      AND EXISTS (
          -- User answered this question wrong
          SELECT 1
          FROM QuestionAnswer qa2
          INNER JOIN UserQuestionAnswer uqa ON qa2.QAID = uqa.QAID
          WHERE qa2.QuestionID = q.QuestionID
            AND uqa.AdriaID = @adriaId
            AND uqa.IsCorrectlyAnswered = FALSE
      )
      AND NOT EXISTS (
          -- User has NEVER answered this question correctly
          SELECT 1
          FROM QuestionAnswer qa3
          INNER JOIN UserQuestionAnswer uqa2 ON qa3.QAID = uqa2.QAID
          WHERE qa3.QuestionID = q.QuestionID
            AND uqa2.AdriaID = @adriaId
            AND uqa2.IsCorrectlyAnswered = TRUE
      )
    ORDER BY q.QuestionID, a.AnswerID;
";
    
    private static readonly string sql_moduleQuestions = @"
    SELECT DISTINCT 
        q.QuestionID,
        q.Question,
        a.AnswerID,
        a.Answer
    FROM QuestionAnswer qa
    INNER JOIN Question q ON qa.QuestionID = q.QuestionID
    INNER JOIN Answer a ON qa.AnswerID = a.AnswerID
    WHERE qa.ModuleID = @moduleId
    ORDER BY q.QuestionID, a.AnswerID;
";

    private readonly DbProviderFactory _factory;
    private readonly string _connectionString;
    private readonly ILogger<GetNextCourseModuleQuery> _logger;

    public GetNextCourseModuleQuery(
        DbProviderFactory factory, 
        string connectionString, 
        ILogger<GetNextCourseModuleQuery> logger)
    {
        _factory = factory;
        _connectionString = connectionString;
        _logger = logger;
    }
    
    public async Task<CourseModuleData?> Fetch(Guid courseId, Guid userId)
{
    _logger.LogInformation(
        "Fetching next module for user {UserId} in course {CourseId}", 
        userId, 
        courseId
    );

    using var connection = _factory.CreateConnection() 
        ?? throw new InvalidOperationException("DbProviderFactory returned null connection");
    
    connection.ConnectionString = _connectionString;
    await connection.OpenAsync();

    Guid? moduleId = null;
    int? position = null;
    string? content = null;
    bool isReview = false;

    // Step 1: Check for modules with wrong answers
    using (var command = connection.CreateCommand())
    {
        command.CommandText = sql_moduleWithWrongAnswers;
        
        var adriaIdParam = command.CreateParameter();
        adriaIdParam.ParameterName = "@adriaId";
        adriaIdParam.Value = userId.ToString();
        command.Parameters.Add(adriaIdParam);
        
        var courseIdParam = command.CreateParameter();
        courseIdParam.ParameterName = "@courseId";
        courseIdParam.Value = courseId.ToString();
        command.Parameters.Add(courseIdParam);

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            moduleId = Guid.Parse(reader.GetString(0));
            position = reader.GetInt32(1);
            content = reader.GetString(2);
            isReview = true; // This is a review module
        }
    }

    // Step 2: No wrong answers? Get the next fresh module
    if (moduleId == null)
    {
        using var command = connection.CreateCommand();
        command.CommandText = sql_nextModule;
        
        var adriaIdParam = command.CreateParameter();
        adriaIdParam.ParameterName = "@adriaId";
        adriaIdParam.Value = userId.ToString();
        command.Parameters.Add(adriaIdParam);
        
        var courseIdParam = command.CreateParameter();
        courseIdParam.ParameterName = "@courseId";
        courseIdParam.Value = courseId.ToString();
        command.Parameters.Add(courseIdParam);

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            moduleId = Guid.Parse(reader.GetString(0));
            position = reader.GetInt32(1);
            content = reader.GetString(2);
        }
    }

    // Step 3: No modules left? Return null
    if (moduleId == null)
    {
        _logger.LogInformation("No more modules to complete for user {UserId} in course {CourseId}", 
            userId, courseId);
        return null;
    }

    // Step 4: Fetch questions - ONLY wrong ones if reviewing, ALL if new module
    var questions = new Dictionary<Guid, (string Question, List<QuestionAnswerData> Answers)>();

    using (var command = connection.CreateCommand())
    {
        // Use different query based on whether it's a review
        command.CommandText = isReview ? sql_wrongQuestions : sql_moduleQuestions;
        
        var moduleIdParam = command.CreateParameter();
        moduleIdParam.ParameterName = "@moduleId";
        moduleIdParam.Value = moduleId.Value.ToString();
        command.Parameters.Add(moduleIdParam);

        if (isReview)
        {
            var adriaIdParam = command.CreateParameter();
            adriaIdParam.ParameterName = "@adriaId";
            adriaIdParam.Value = userId.ToString();
            command.Parameters.Add(adriaIdParam);
        }

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var questionId = Guid.Parse(reader.GetString(0));
            var questionText = reader.GetString(1);
            var answerId = Guid.Parse(reader.GetString(2));
            var answerText = reader.GetString(3);

            if (!questions.TryGetValue(questionId, out var entry))
            {
                entry = (questionText, new List<QuestionAnswerData>());
                questions[questionId] = entry;
            }

            entry.Answers.Add(new QuestionAnswerData(answerId, answerText));
        }
    }

    var moduleQuestions = questions
        .Select(kvp => new ModuleQuestionData(
            kvp.Key,
            kvp.Value.Question,
            kvp.Value.Answers
        ))
        .ToList();

    return new CourseModuleData(
        moduleId.Value,
        position!.Value,
        content!,
        moduleQuestions
    );
}

}