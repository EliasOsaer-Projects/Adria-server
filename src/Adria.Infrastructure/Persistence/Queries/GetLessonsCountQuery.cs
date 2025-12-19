using System.Data.Common;
using Adria.Application.Contracts;
using Adria.Infrastructure.Persistence.Queries;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Courses;

public class GetLessonsCountQuery
    : IGetLessonsCountQuery
{
    private static readonly string QRY = @"
        SELECT COUNT(*) as lessonsCount
        FROM Module
        WHERE CourseId = @CourseId;
    ";

    private readonly DbProviderFactory _factory;
    private readonly string _connectionString;
    private readonly ILogger<GetLessonsCountQuery> _logger;

    public GetLessonsCountQuery(
        DbProviderFactory factory,
        string connectionString,
        ILogger<GetLessonsCountQuery> logger)
    {
        _factory = factory;
        _connectionString = connectionString;
        _logger = logger;
    }
    
    public async Task<int> Fetch(Guid courseId)
    {
        _logger.LogInformation("Fetching lessons count for course {CourseId}", courseId);
        
        using var connection = _factory.CreateConnection()
            ?? throw new InvalidOperationException("DBFactory provided a null connection");

        connection.ConnectionString = _connectionString;
        await connection.OpenAsync();
        
        using var command = connection.CreateCommand();
        command.CommandText = QRY;
        
        var parameter = command.CreateParameter();
        parameter.ParameterName = "@CourseId";
        parameter.Value = courseId.ToString();
        
        command.Parameters.Add(parameter);
        
        var result = await command.ExecuteScalarAsync();
        return result is null ? 0 : Convert.ToInt32(result);
    }
}