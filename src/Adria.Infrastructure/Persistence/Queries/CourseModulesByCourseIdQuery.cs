using System.Data.Common;
using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Microsoft.Extensions.Logging;

namespace Adria.Infrastructure.Persistence.Queries;

public sealed class CourseModulesByCourseIdQuery : ICourseModulesByCourseIdQuery
{
    private static readonly string sql = @"
        SELECT 
            m.ModuleID,
            m.Position,
            m.Content,
            q.QuestionID,
            q.Question,
            a.AnswerID,
            a.Answer,
            a.IsCorrect
        FROM Module m
        LEFT JOIN QuestionAnswer qa ON qa.ModuleID = m.ModuleID
        LEFT JOIN Question q ON q.QuestionID = qa.QuestionID
        LEFT JOIN Answer a ON a.AnswerID = qa.AnswerID
        WHERE m.CourseID = @CourseId
        ORDER BY m.Position, q.QuestionID;
    ";

    private readonly DbProviderFactory _factory;
    private readonly string _connectionString;
    private readonly ILogger<CourseModulesByCourseIdQuery> _logger;

    public CourseModulesByCourseIdQuery(DbProviderFactory factory, string connectionString, ILogger<CourseModulesByCourseIdQuery> logger)
    {
        _factory = factory;
        _connectionString = connectionString;
        _logger = logger;
    }

    public async Task<IEnumerable<CourseModuleData>> Fetch(Guid courseId)
    {
        _logger.LogInformation("Fetching modules for course {CourseId}", courseId);

        using var connection = _factory.CreateConnection() ?? throw new InvalidOperationException("DbProviderFactory returned null connection");
        connection.ConnectionString = _connectionString;
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = sql;

        var param = command.CreateParameter();
        param.ParameterName = "@CourseId";
        param.Value = courseId.ToString();
        command.Parameters.Add(param);

        var modules = new Dictionary<Guid, CourseModuleData>();

        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var moduleId = reader.GetGuid(reader.GetOrdinal("ModuleID"));
            if (!modules.TryGetValue(moduleId, out var module))
            {
                module = new CourseModuleData(
                    ModuleId: moduleId,
                    Position: reader.GetInt32(reader.GetOrdinal("Position")),
                    Content: reader.GetString(reader.GetOrdinal("Content")),
                    Questions: new List<ModuleQuestionData>()
                );
                modules[moduleId] = module;
            }

            if (!await reader.IsDBNullAsync(reader.GetOrdinal("QuestionID")))
            {
                var questionId = reader.GetGuid(reader.GetOrdinal("QuestionID"));
                var questionText = reader.GetString(reader.GetOrdinal("Question"));
                var answers = new List<QuestionAnswerData>();

                if (!await reader.IsDBNullAsync(reader.GetOrdinal("AnswerID")))
                {
                    answers.Add(new QuestionAnswerData(
                        AnswerId: reader.GetGuid(reader.GetOrdinal("AnswerID")),
                        Answer: reader.GetString(reader.GetOrdinal("Answer")),
                        IsCorrect: reader.GetBoolean(reader.GetOrdinal("IsCorrect"))
                    ));
                }

                ((List<ModuleQuestionData>)module.Questions).Add(new ModuleQuestionData(
                    QuestionId: questionId,
                    Question: questionText,
                    Answers: answers
                ));
            }
        }

        return modules.Values;
    }
}