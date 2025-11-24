using System.Data.Common;
using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Microsoft.Extensions.Logging;

namespace Adria.Infrastructure.Persistence.Queries;

public class EnrollUserInCourseQuery
    : IEnrollUserInCourseQuery
{
    private static readonly string QRY = @"
        INSERT INTO UserCourse (AdriaID, CourseID) VALUES (@userId, @courseId)
    ";

    private static readonly string QRY2 = @"
        SELECT
        uc.CourseID,
        c.CourseName,
        c.CourseDifficulty,
        uc.ProgressPercentage,
        uc.CompletionStatus
        FROM UserCourse uc
        JOIN Course c ON uc.CourseID = c.CourseID
        WHERE uc.AdriaID = @userID AND uc.CourseID = @courseId
    ";
    
    private readonly DbProviderFactory _factory;
    private readonly string _connectionString;
    private readonly ILogger<EnrollUserInCourseQuery> _logger;
    
    public EnrollUserInCourseQuery(
        DbProviderFactory factory,
        string connectionString,
        ILogger<EnrollUserInCourseQuery> logger)
    {
        _factory = factory;
        _connectionString = connectionString;
        _logger = logger;
    }
    
    public async Task<UserCourseData> Fetch(Guid userId, Guid courseId)
    {
        _logger.LogInformation("Enrolling user {UserId} in course {CourseId}", userId, courseId);
        
        using var connection = _factory.CreateConnection() ?? throw new InvalidOperationException("DbProviderFactory returned a null DbConnection.");
        connection.ConnectionString = _connectionString;
        await connection.OpenAsync();
        
        using var command = connection.CreateCommand();
        using var command2 = connection.CreateCommand();

        command.CommandText = QRY;
        command2.CommandText = QRY2;
        
        var userIdParameter = command.CreateParameter();
        userIdParameter.ParameterName = "@userId";
        userIdParameter.Value = userId;
        command.Parameters.Add(userIdParameter);
        
        var courseIdParameter = command.CreateParameter();
        courseIdParameter.ParameterName = "@courseId";
        courseIdParameter.Value = courseId;
        command.Parameters.Add(courseIdParameter);
        
        var userIdParameter2 = command.CreateParameter();
        userIdParameter2.ParameterName = "@userId";
        userIdParameter2.Value = userId;
        command2.Parameters.Add(userIdParameter2);
        
        var courseIdParameter2 = command.CreateParameter();
        courseIdParameter2.ParameterName = "@courseId";
        courseIdParameter2.Value = courseId;
        command2.Parameters.Add(courseIdParameter2);

        int rows =  await command.ExecuteNonQueryAsync();
        await using var reader = await command2.ExecuteReaderAsync();

        if (rows == 0 || !await reader.ReadAsync())
        {
            _logger.LogInformation("User {UserId} not enrolled in course {CourseId}", userId, courseId);
        }

        return new UserCourseData(
            CourseId: reader.GetGuid(reader.GetOrdinal("CourseID")),
            CourseName: reader.GetString(reader.GetOrdinal("CourseName")),
            Level: reader.GetInt16(reader.GetOrdinal("CourseDifficulty")),
            ProgressPercentage: reader.GetInt32(reader.GetOrdinal("ProgressPercentage")),
            CompletionStatus: reader.GetBoolean(reader.GetOrdinal("CompletionStatus"))
        );
    }
}