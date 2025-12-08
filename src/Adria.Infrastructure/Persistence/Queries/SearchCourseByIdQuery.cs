using System.Data.Common;
using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Domain.Shared.Helpers;
using Microsoft.Extensions.Logging;

namespace Adria.Infrastructure.Persistence.Queries;

public sealed class SearchCourseByIdQuery
    : ISearchCourseByIdQuery
{
    private static readonly string QRY = @"
        SELECT * 
        FROM Course 
        JOIN BannerImage ON Course.BannerImageID = BannerImage.BannerImageID
        WHERE Course.CourseID = @id
    ";
    
    private readonly DbProviderFactory _factory;
    private readonly string _connectionString;
    private readonly ILogger<SearchCourseByIdQuery> _logger;

    public SearchCourseByIdQuery(
        DbProviderFactory factory,
        string connectionString,
        ILogger<SearchCourseByIdQuery> logger
    )
    {
        _factory = factory;
        _connectionString = connectionString;
        _logger = logger;
    }

    public async Task<CourseData> Fetch(Guid id)
    {
        _logger.LogInformation("Fetching course with ID {Id}", id);
        
        using var connection = _factory.CreateConnection() ?? throw new InvalidOperationException("DBFactory provided a null connection");
        connection.ConnectionString = _connectionString;
        await connection.OpenAsync();
        
        using var command = connection.CreateCommand();
        command.CommandText = QRY;
        
        var parameter = command.CreateParameter();
        parameter.ParameterName = "@id";
        parameter.Value = id;
        command.Parameters.Add(parameter);
        
        await using var reader = await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
        {
            _logger.LogInformation("Course with ID {Id} not found", id);
            return null!;
        }

        return new CourseData(
            Id: reader.GetGuid(reader.GetOrdinal("CourseID")),
            Name: reader.GetString(reader.GetOrdinal("CourseName")),
            Level: reader.GetInt16(reader.GetOrdinal("CourseDifficulty")),
            Description: reader.GetString(reader.GetOrdinal("CourseDescription")),
            Category: reader.GetString(reader.GetOrdinal("CourseCategory")),
            Tags: reader.GetString(reader.GetOrdinal("CourseTags")).Split(',', StringSplitOptions.TrimEntries).ToList(),
            PointsOnCompletion: reader.GetInt32(reader.GetOrdinal("PointsOnCompletion")),
            LearnList: reader.GetString(reader.GetOrdinal("LearnList")).Split(',', StringSplitOptions.TrimEntries).ToList(),
            Duration: reader.GetInt32(reader.GetOrdinal("EstimatedDurationInHours")),
            SubscriptionRequired: reader.GetBoolean(reader.GetOrdinal("SubscriptionRequired")),
            CoverImage: Array.Empty<byte>(),
            BannerImage: await Helper.ReadBlobAsync(reader, reader.GetOrdinal("BannerImage"))
        );
    }
}