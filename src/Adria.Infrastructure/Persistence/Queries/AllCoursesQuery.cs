using System.Data.Common;
using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Microsoft.Extensions.Logging;

namespace Adria.Infrastructure.Persistence.Queries;

public sealed class AllCoursesQuery : IAllCoursesQuery
{
    private static readonly string QRY = @"
        SELECT *
        From Courses
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
            var ordCourseCategory = reader.GetOrdinal("CourseCategory");
            var ordPointsOnCompletion = reader.GetOrdinal("PointsOnCompletion");
            var ordEstimatedDuration = reader.GetOrdinal("EstimatedDurationInHours");
            var ordCoverImageId = reader.GetOrdinal("CoverImageID");
            var ordBannerImageId = reader.GetOrdinal("BannerImageID");

            courses.Add(new CourseData(
                Id: reader.GetGuid(ordCourseId),
                Name: reader.GetString(ordCourseName),
                Level: reader.GetInt32(ordCourseDifficulty),
                Category: reader.GetString(ordCourseCategory),
                Description: reader.GetString(ordCourseDescription),
                PointsOnCompletion: reader.GetInt32(ordPointsOnCompletion),
                Duration: reader.GetInt32(ordEstimatedDuration),
                LearnList: [],
                Modules: []
            ));
        }
        

        _logger.LogInformation("Fetched {Count} courses", courses.Count);

        return courses.AsReadOnly();
    }

}