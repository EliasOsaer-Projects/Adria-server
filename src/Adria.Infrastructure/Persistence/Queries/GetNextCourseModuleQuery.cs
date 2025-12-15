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
            AND uqa.IsCorrectlyAnswered = TRUE
      )
    ORDER BY m.Position ASC
    LIMIT 1;
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

    public async Task<CourseModuleData?> Fetch(Guid courseId, Guid adriaId)
    {
        _logger.LogInformation(
            "Fetching next module for user {AdriaId} in course {CourseId}", 
            adriaId, 
            courseId
        );

        using var connection = _factory.CreateConnection() 
            ?? throw new InvalidOperationException("DbProviderFactory returned null connection");
        
        connection.ConnectionString = _connectionString;
        await connection.OpenAsync();

        // Check for modules with wrong answers
        Guid? moduleId = null;
        int? position = null;
        string? content = null;

        using (var command = connection.CreateCommand())
        {
            command.CommandText = sql_moduleWithWrongAnswers;
            
            var adriaIdParam = command.CreateParameter();
            adriaIdParam.ParameterName = "@adriaId";
            adriaIdParam.Value = adriaId.ToString();
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

        // No wrong answers? get the next module
        if (moduleId == null)
        {
            using var command = connection.CreateCommand();
            command.CommandText = sql_nextModule;
            
            var adriaIdParam = command.CreateParameter();
            adriaIdParam.ParameterName = "@adriaId";
            adriaIdParam.Value = adriaId.ToString();
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

        // no modules left? return null
        if (moduleId == null)
        {
            _logger.LogInformation("No more modules to complete for user {AdriaId} in course {CourseId}", 
                adriaId, courseId);
            return null;
        }

        // get questions and answers for the module
        var questions = new Dictionary<Guid, (string Question, List<QuestionAnswerData> Answers)>();

        using (var command = connection.CreateCommand())
        {
            command.CommandText = sql_moduleQuestions;
            
            var moduleIdParam = command.CreateParameter();
            moduleIdParam.ParameterName = "@moduleId";
            moduleIdParam.Value = moduleId.Value.ToString();
            command.Parameters.Add(moduleIdParam);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var questionId = Guid.Parse(reader.GetString(0));
                var questionText = reader.GetString(1);
                var answerId = Guid.Parse(reader.GetString(2));
                var answerText = reader.GetString(3);

                if (!questions.ContainsKey(questionId))
                {
                    questions[questionId] = (questionText, new List<QuestionAnswerData>());
                }

                questions[questionId].Answers.Add(new QuestionAnswerData(answerId, answerText));
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