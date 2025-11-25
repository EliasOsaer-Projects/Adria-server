using System.Data.Common;
using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Domain;
using Adria.Domain.Shared.Helpers;
using Microsoft.Extensions.Logging;

namespace Adria.Infrastructure.Persistence.Queries;

public sealed class AllCoursesQuery : IAllCoursesQuery
{
    private static readonly string QRY = @"
        SELECT *
        From Course
        JOIN CoverImage ON Course.CoverImageID = CoverImage.CoverImageID
    ";


    private readonly DbProviderFactory _factory;
    private readonly string _connectionString;
    private readonly ILogger<AllCoursesQuery> _logger;

    public AllCoursesQuery(
        DbProviderFactory factory,
        string connectionString,
        ILogger<AllCoursesQuery> logger)
    {
        _factory = factory;
        _connectionString = connectionString;
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<CourseData>> Fetch()
    {
        _logger.LogInformation("Starting to fetch all courses");

        using var connection = _factory.CreateConnection()
                               ?? throw new InvalidOperationException(
                                   "DbProviderFactory returned a null DbConnection.");

        connection.ConnectionString = _connectionString;
        await connection.OpenAsync();

        using var command = connection.CreateCommand();

        command.CommandText = QRY;

        using var reader = await command.ExecuteReaderAsync();

        var courses = new List<CourseData>();

        while (await reader.ReadAsync())
        {
            var ordCourseId = reader.GetOrdinal("CourseID");
            var ordCourseName = reader.GetOrdinal("CourseName");
            var ordCourseDifficulty = reader.GetOrdinal("CourseDifficulty");
            var ordCourseDescription = reader.GetOrdinal("CourseDescription");
            var ordCourseTags = reader.GetOrdinal("CourseTags");
            var ordCourseCategory = reader.GetOrdinal("CourseCategory");
            var ordPointsOnCompletion = reader.GetOrdinal("PointsOnCompletion");
            var ordEstimatedDuration = reader.GetOrdinal("EstimatedDurationInHours");
            var ordLearnList = reader.GetOrdinal("LearnList");
            var ordCoverImage = reader.GetOrdinal("CoverImage");
            var ordSubscriptionRequired = reader.GetOrdinal("SubscriptionRequired");
            
            byte[] coverImageBytes = await Helper.ReadBlobAsync(reader, ordCoverImage);
            courses.Add(new CourseData(
                Id: reader.GetGuid(ordCourseId),
                Name: reader.GetString(ordCourseName),
                Level: reader.GetInt16(ordCourseDifficulty),
                Description: reader.GetString(ordCourseDescription),
                Category: reader.GetString(ordCourseCategory),
                Tags: reader.GetString(ordCourseTags).Split(',', StringSplitOptions.TrimEntries).ToList(),
                PointsOnCompletion: reader.GetInt32(ordPointsOnCompletion),
                Duration: reader.GetInt32(ordEstimatedDuration),
                LearnList: reader.GetString(ordLearnList).Split(',', StringSplitOptions.TrimEntries).ToList(),
                SubscriptionRequired: reader.GetBoolean(ordSubscriptionRequired),
                CoverImage: coverImageBytes,
                BannerImage: Array.Empty<byte>()
            ));
        }
        

        _logger.LogInformation("Fetched {Count} courses", courses.Count);

        return courses.AsReadOnly();
    }
    
    
}