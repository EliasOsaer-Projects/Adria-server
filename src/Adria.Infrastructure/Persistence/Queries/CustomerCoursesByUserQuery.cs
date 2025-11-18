using System.Collections.ObjectModel;
using System.Data.Common;
using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Microsoft.Extensions.Logging;

namespace Adria.Infrastructure.Persistence.Queries;

public sealed class CustomerCoursesByUserQuery : ICustomerCoursesByUserQuery
{
    private static readonly string QRY = @"
       SELECT
    u.AdriaID,
    u.Name AS UserName,
    u.Wallet,
    u.PointsBalance,
    u.Streak,
    cc.CustomerCourseID,
    cc.CompletionStatus,
    cc.ProgressPercentage,
    cc.StartDate AS CourseStartDate,
    cc.CompletionDate AS CourseCompletionDate,
    c.CourseID,
    c.CourseName,
    c.CourseDifficulty,
    c.CourseDescription,
    c.CourseCategory,
    c.PointsOnCompletion,
    c.EstimatedDurationInHours,
    c.SubscriptionRequired,
    c.CourseTags
FROM User u
JOIN CustomerCourse cc ON u.AdriaID = cc.AdriaID
JOIN Courses c ON cc.CourseID = c.CourseID
WHERE u.AdriaID = @userID
ORDER BY c.CourseName;
    ";


    private readonly DbProviderFactory _factory;
    private readonly string _connectionString;
    private readonly ILogger<CustomerCoursesByUserQuery> _logger;

    public CustomerCoursesByUserQuery(
        DbProviderFactory factory,
        string connectionString,
        ILogger<CustomerCoursesByUserQuery> logger)
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
        

        var dataDict = new Dictionary<string, UserData>();
        

        while (await reader.ReadAsync())
        {
            var adriaID = reader.GetString(reader.GetOrdinal("AdriaID"));
            
            //hier wordt gecheckt of de user al bestaat omdat er anders meerdere keren dezelfde user wordt gegeven maar dan met 1 course.
            if (!dataDict.TryGetValue(adriaID, out var userData))
            {
                userData = new UserData(
                    AdriaId: adriaID,
                    UserName: reader.GetString(reader.GetOrdinal("UserName")),
                    Wallet: reader.GetDecimal(reader.GetOrdinal("Wallet")),
                    PointsBalance: reader.GetInt32(reader.GetOrdinal("PointsBalance")),
                    LastPayment: reader.IsDBNull(reader.GetOrdinal("LastPayment"))
                        ? null
                        : reader.GetDateTime(reader.GetOrdinal("LastPayment")),
                    Streak: reader.GetInt32(reader.GetOrdinal("Streak")),
                    Subscription: null,
                    CustomerCourses: new List<CustomerCourseData>()
                );

                dataDict[adriaID] = userData;
            }
            
            var tags = reader.GetString(reader.GetOrdinal("CourseTags"))
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(tag => tag.Trim())
                .ToList();

            var courseData = new CourseData(
                CourseId: reader.GetGuid(reader.GetOrdinal("CourseID")),
                CourseName: reader.GetString(reader.GetOrdinal("CourseName")),
                Category: reader.GetString(reader.GetOrdinal("CourseCategory")),
                Description: reader.GetString(reader.GetOrdinal("CourseDescription")),
                Tags: tags,
                LearnList: [],
                Duration: reader.GetInt32(reader.GetOrdinal("EstimatedDurationInHours")),
                Level: reader.GetInt32(reader.GetOrdinal("CourseDifficulty")),
                Modules: [],
                PointsOnCompletion: reader.GetInt32(reader.GetOrdinal("PointsOnCompletion"))
            );

            userData.CustomerCourses.Add(new CustomerCourseData(
                CustomerCourseId: reader.GetGuid(reader.GetOrdinal("CustomerCourseID")),
                Course: courseData,
                CompletionStatus: reader.GetBoolean(reader.GetOrdinal("CompletionStatus")),
                Progress: (decimal)reader.GetInt32(reader.GetOrdinal("ProgressPercentage"))
            ));
        }
        
        var data = dataDict.Values.ToList().AsReadOnly();
        if (data != null)
            _logger.LogInformation("Fetched user {UserId} with {Count} courses", id, data.First().CustomerCourses.Count);
        else
            _logger.LogWarning("No user found with ID {UserId}", id);
        return data;
    }
}


