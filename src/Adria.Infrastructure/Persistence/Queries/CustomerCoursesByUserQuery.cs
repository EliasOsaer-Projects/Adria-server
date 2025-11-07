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

        using var reader = await command.ExecuteReaderAsync();

        var data = new List<UserData>();

        while (await reader.ReadAsync())
        {
            var ordAdriaID = reader.GetOrdinal("AdriaID");
            var ordUserName = reader.GetOrdinal("UserName");
            var ordWallet = reader.GetOrdinal("Wallet");
            var ordPointsBalance = reader.GetOrdinal("PointsBalance");
            var ordLastPayment =  reader.GetOrdinal("LastPayment");
            var ordStreak = reader.GetOrdinal("Streak");

            var ordCustomerCourseID = reader.GetOrdinal("CustomerCourseID");
            var ordCompletionStatus = reader.GetOrdinal("CompletionStatus");
            var ordProgressPercentage = reader.GetOrdinal("ProgressPercentage");
            var ordCourseStartDate = reader.GetOrdinal("CourseStartDate");
            var ordCourseCompletionDate = reader.GetOrdinal("CourseCompletionDate");

            var ordCourseID = reader.GetOrdinal("CourseID");
            var ordCourseName = reader.GetOrdinal("CourseName");
            var ordCourseDifficulty = reader.GetOrdinal("CourseDifficulty");
            var ordCourseDescription = reader.GetOrdinal("CourseDescription");
            var ordCourseCategory = reader.GetOrdinal("CourseCategory");
            var ordPointsOnCompletion = reader.GetOrdinal("PointsOnCompletion");
            var ordEstimatedDurationInHours = reader.GetOrdinal("EstimatedDurationInHours");
            var ordSubscriptionRequired = reader.GetOrdinal("SubscriptionRequired");
            var ordCourseTags = reader.GetOrdinal("CourseTags");


            List<string> tags = reader.GetString(ordCourseTags)
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(tag => tag.Trim())
                .ToList();

            var courseData = new CourseData(
                CourseId: reader.GetGuid(ordCourseID),
                CourseName: reader.GetString(ordCourseName),
                Category: reader.GetString(ordCourseCategory),
                Description: reader.GetString(ordCourseDescription),
                Tags: tags,
                LearnList: [],
                Duration: reader.GetInt32(ordEstimatedDurationInHours),
                Level: reader.GetInt32(ordCourseDifficulty),
                Modules: [],
                PointsOnCompletion: reader.GetInt32(ordPointsOnCompletion)
            );


            var currentData = new UserData(
                AdriaId: reader.GetGuid(ordAdriaID).ToString(),
                UserName: reader.GetString(ordUserName),
                Wallet: reader.GetDecimal(ordWallet),
                PointsBalance: reader.GetInt32(ordPointsBalance),
                LastPayment: reader.IsDBNull(ordLastPayment) ? null : reader.GetDateTime(ordLastPayment),
                Streak: reader.GetInt32(ordStreak),
                Subscription: null, 
                CustomerCourses: []
            );
            
            do
            {
                if (!await reader.IsDBNullAsync(ordCustomerCourseID))
                {
                    currentData.CustomerCourses.Add(new CustomerCourseData(
                        CustomerCourseId: reader.GetGuid(ordCustomerCourseID),
                        Course: courseData,
                        CompletionStatus: reader.GetBoolean(ordCompletionStatus),
                        Progress: (decimal)reader.GetInt32(ordProgressPercentage)
                    ));
                }
            } while (await reader.ReadAsync());
            
            data.Add(currentData);
        }

        
        _logger.LogInformation("Fetched {Count} Users", data.Count);

        return data.AsReadOnly();
    }
}