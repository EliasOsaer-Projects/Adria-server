using System.Collections.ObjectModel;
using System.Data.Common;
using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Domain;
using Microsoft.Extensions.Logging;

namespace Adria.Infrastructure.Persistence.Queries;

public sealed class SearchUserCoursesByUserQuery : IUserCoursesByUserQuery
{
    private static readonly string QRY = @"
        SELECT
        c.CourseID,
        c.CourseName,
        c.CourseDifficulty,
        uc.CompletionStatus,
        uc.ProgressPercentage
        FROM UserCourse uc
        JOIN Course c ON uc.CourseID = c.CourseID
        WHERE uc.AdriaID = @userID
        ORDER BY uc.StartDate DESC
    ";


    private readonly DbProviderFactory _factory;
    private readonly string _connectionString;
    private readonly ILogger<SearchUserCoursesByUserQuery> _logger;

    public SearchUserCoursesByUserQuery(
        DbProviderFactory factory,
        string connectionString,
        ILogger<SearchUserCoursesByUserQuery> logger)
    {
        _factory = factory;
        _connectionString = connectionString;
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<UserData>> Fetch(string id)
    {
        _logger.LogInformation("Starting to fetch all customer courses");

        using var connection = _factory.CreateConnection()
                               ?? throw new InvalidOperationException(
                                   "DbProviderFactory returned a null DbConnection.");

        connection.ConnectionString = _connectionString;
        await connection.OpenAsync();

        using var command = connection.CreateCommand();

        command.CommandText = QRY;


        var param = command.CreateParameter();
        param.ParameterName = "@userID";
        param.Value = id;
        command.Parameters.Add(param);


        using var reader = await command.ExecuteReaderAsync();
        

        IList<UserCourseData> userCourseDataList = new List<UserCourseData>();
        

        while (await reader.ReadAsync())
        {
            var userCourse = new UserCourseData(
                CourseId: reader.GetGuid(reader.GetOrdinal("CourseID")),
                CourseName: reader.GetString(reader.GetOrdinal("CourseName")),
                Difficulty: reader.GetInt16(reader.GetOrdinal("CourseDifficulty")),
                CompletionStatus: reader.GetBoolean(reader.GetOrdinal("CompletionStatus")),
                ProgressPercentage: reader.GetInt32(reader.GetOrdinal("ProgressPercentage"))
            );
            userCourseDataList.Add(userCourse);
        }


        if (userCourseDataList != null)
        {
            _logger.LogInformation("Fetched user {UserId} with {Count} courses", id, userCourseDataList.Count);
        }
        else
        {
            _logger.LogWarning("No user found with ID {UserId}", id);
        }
        return userCourseDataList.AsReadOnly();
    }
}


