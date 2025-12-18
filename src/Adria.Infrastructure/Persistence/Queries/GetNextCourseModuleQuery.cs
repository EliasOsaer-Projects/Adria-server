using System.Data.Common;
using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Domain;
using Microsoft.Extensions.Logging;
using Taskly.Infrastructure.Persistence.Shared;

namespace Adria.Infrastructure.Persistence.Queries;

public sealed class GetNextCourseModuleQuery 
    : IGetNextCourseModuleQuery
{
    private static readonly string CHECK_FOR_UNANSWERED_OR_WRONG_QUESTIONS_QUERY = @"
        SELECT DISTINCT 
           m.ModuleID,
           m.Position,
           m.Content
        FROM Module m
        JOIN QuestionAnswer qa ON qa.ModuleID = m.ModuleID
        WHERE m.CourseID = @CourseId
          AND EXISTS (
              SELECT 1
              FROM QuestionAnswer qa1
              LEFT JOIN UserQuestionAnswer uqa1
                     ON uqa1.QAID = qa1.QAID AND uqa1.AdriaID = @UserId
              WHERE qa1.ModuleID = m.ModuleID
               
                AND NOT EXISTS (
                    SELECT 1
                    FROM QuestionAnswer qa2
                    JOIN UserQuestionAnswer uqa2
                      ON uqa2.QAID = qa2.QAID
                     AND uqa2.AdriaID = @UserId
                     AND uqa2.IsCorrectlyAnswered = 1
                    WHERE qa2.QuestionID = qa1.QuestionID
                )
          )
        ORDER BY m.Position
        LIMIT 1;
    ";
    
    private static readonly string GET_NEXT_MODULE_QUERY = @"
        SELECT 
            *
        FROM QuestionAnswer qa
        JOIN Question q ON q.QuestionID = qa.QuestionID
        JOIN Answer   a ON a.AnswerID   = qa.AnswerID
        WHERE qa.ModuleID = @ModuleId
          AND NOT EXISTS (
                SELECT 1
                FROM QuestionAnswer qa_ok
                JOIN UserQuestionAnswer uqa_ok
                     ON uqa_ok.QAID = qa_ok.QAID
                WHERE qa_ok.QuestionID = qa.QuestionID
                  AND uqa_ok.AdriaID = @UserId
                  AND uqa_ok.IsCorrectlyAnswered = 1
          )
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
    
    private async Task<IReadOnlyCollection<ModuleQuestionData>> GetQuestions(Guid moduleId, Guid userId)
    {
        var questions = new Dictionary<Guid, (string Question, List<QuestionAnswerData> Answers)>();
        
        using var connection = _factory.CreateConnection() 
                               ?? throw new AdriaDatabaseException("DbProviderFactory returned a null DbConnection.");
        
        connection.ConnectionString = _connectionString;
        await connection.OpenAsync();
        
        using var command = connection.CreateCommand();
        command.CommandText = GET_NEXT_MODULE_QUERY;
        
        var moduleIdParam = command.CreateParameter();
        moduleIdParam.ParameterName = "@ModuleId";
        moduleIdParam.Value = moduleId;
        command.Parameters.Add(moduleIdParam);
        
        var userIdParam = command.CreateParameter();
        userIdParam.ParameterName = "@UserId";
        userIdParam.Value = userId;
        command.Parameters.Add(userIdParam);
        
        await using var reader = await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
        {
            _logger.LogInformation("No questions found for module {ModuleId}", moduleId);
            return Array.Empty<ModuleQuestionData>();
        }

        while (await reader.ReadAsync())
        {
            var questionId = reader.GetGuid(reader.GetOrdinal("QuestionID"));
            var questionText = reader.GetString(reader.GetOrdinal("Question"));
            var answerId = reader.GetGuid(reader.GetOrdinal("AnswerID"));
            var answerText = reader.GetString(reader.GetOrdinal("Answer"));

            if (!questions.TryGetValue(questionId, out var entry))
            {
                entry = (questionText, new List<QuestionAnswerData>());
                questions.Add(questionId, entry);
            }

            entry.Answers.Add(new QuestionAnswerData(answerId, answerText));
        }
        
        if (questions.Count == 0)
        {
            _logger.LogInformation("No questions found for module {ModuleId}", moduleId);
            return Array.Empty<ModuleQuestionData>();
        }
        
        return questions
            .Select(questionEntry => new ModuleQuestionData(
                QuestionId: questionEntry.Key,
                Question: questionEntry.Value.Question,
                Answers: questionEntry.Value.Answers.AsReadOnly()
            ))
            .ToList()
            .AsReadOnly();
    }
    
    public async Task<CourseModuleData?> Fetch(Guid courseId, Guid userId)
    {
        _logger.LogInformation("Fetching next course module for course {CourseId} and user {UserId}", courseId, userId);

        try
        {
            _logger.LogInformation("Fetching modules with unanswered or wrong questions for course {CourseId} and user {UserId}", courseId, userId);

            using var connection = _factory.CreateConnection()
                                   ?? throw new AdriaDatabaseException("DbProviderFactory returned a null DbConnection.");
        
            connection.ConnectionString = _connectionString;
            await connection.OpenAsync();
        
            using var command = connection.CreateCommand();
            command.CommandText = CHECK_FOR_UNANSWERED_OR_WRONG_QUESTIONS_QUERY;
        
            var courseIdParam = command.CreateParameter();
            courseIdParam.ParameterName = "@CourseId";
            courseIdParam.Value = courseId;
            command.Parameters.Add(courseIdParam);
        
            var userIdParam = command.CreateParameter();
            userIdParam.ParameterName = "@UserId";
            userIdParam.Value = userId;
            command.Parameters.Add(userIdParam);
        
            await using var reader = await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
            {
                _logger.LogInformation("No module with wrong or unanswered questions");
                throw new GenesisException("No module with wrong or unanswered questions");
            }

            return new CourseModuleData(
                ModuleId: reader.GetGuid(reader.GetOrdinal("ModuleID")),
                Position: reader.GetInt32(reader.GetOrdinal("Position")),
                Content: reader.GetString(reader.GetOrdinal("Content")),
                Questions: await GetQuestions(reader.GetGuid(reader.GetOrdinal("ModuleID")), userId)
            );
        }
        catch (GenesisException e)
        {
            throw new GenesisException("No module found");
        }
    }

}