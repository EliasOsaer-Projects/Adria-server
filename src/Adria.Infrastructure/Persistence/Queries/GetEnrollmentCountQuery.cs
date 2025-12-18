using System.Data.Common;
using Adria.Application.Contracts;
using Microsoft.Extensions.Logging;

namespace Adria.Infrastructure.Persistence.Queries;

public class GetEnrollmentCountQuery
    : IGetEnrollmentCountQuery
{
    private static readonly string QRY = @"
        SELECT COUNT(*) AS enrollmentCount
        FROM UserCourse uc
        WHERE uc.CourseID = @CourseId;
    ";


    private readonly DbProviderFactory _factory;
    private readonly string _connectionString;
    private readonly ILogger<GetEnrollmentCountQuery> _logger;

    public GetEnrollmentCountQuery(
        DbProviderFactory factory,
        string connectionString,
        ILogger<GetEnrollmentCountQuery> logger)
    {
        _factory = factory;
        _connectionString = connectionString;
        _logger = logger;
    }
    
    public async Task<int> Fetch(Guid courseId)
    {
        _logger.LogInformation("Fetching enrollment count for course {CourseId}", courseId);
        
        using var connection = _factory.CreateConnection() 
                               ?? throw new InvalidOperationException("DBFactory provided a null connection");
        
        connection.ConnectionString = _connectionString;
        await connection.OpenAsync();
        
        using var command = connection.CreateCommand();
        command.CommandText = QRY;
        
        var param = command.CreateParameter();
        param.ParameterName = "@CourseId";
        param.Value = courseId.ToString();
        command.Parameters.Add(param);
        
        var result = await command.ExecuteScalarAsync();
        
        return result is null ? 0 : Convert.ToInt32(result);
    }
}