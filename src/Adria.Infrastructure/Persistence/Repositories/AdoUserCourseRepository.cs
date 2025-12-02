using System.Data.Common;
using Adria.Domain;
using Adria.Domain.Contracts;
using Adria.Domain.Shared.Helpers;
using Adria.Domain.Users;
using Adria.Infrastructure.Persistence.Shared;
using Microsoft.Extensions.Logging;

namespace Adria.Infrastructure.Persistence.Repositories;

public class AdoUserCourseRepository
    : AbstractAdoRepository, IUserCourseRepository
{
    private readonly ILogger<AdoUserCourseRepository> _logger;

    private static readonly string TABLE_USER_SUBS = "UserCourse";
    private static readonly string COL_USER_ID = "AdriaID";
    private static readonly string COL_SUBS_ID = "CourseID";

    private static readonly string INSERT_USERCOURSE = $@"
        INSERT INTO {TABLE_USER_SUBS} ({COL_USER_ID}, {COL_SUBS_ID})
        VALUES (@UserId, @CourseId);
    ";
    
    private static readonly string SELECT_USERCOURSE = $@"
        SELECT
        uc.CourseID,
        c.CourseName,
        c.CourseDifficulty,
        uc.ProgressPercentage,
        uc.CompletionStatus,
        ci.CoverImage
        FROM UserCourse uc
        JOIN Course c ON uc.CourseID = c.CourseID
        JOIN CoverImage ci ON c.CoverImageID = ci.CoverImageID
        WHERE uc.AdriaID = @userID AND uc.CourseID = @courseId
    ";

    
    public AdoUserCourseRepository
    (
        DbProviderFactory factory,
        string connectionString,
        ILogger<AdoUserCourseRepository> logger
    ) : base(factory, connectionString)
    {
        _logger = logger;
    }

    public async Task<UserCourseRepoData> Save(Guid userId, Guid courseId)
    {
        try
        {
            var parameters = new[]
            {
                CreateParameter("@UserId", userId.ToString()),
                CreateParameter("@CourseId", courseId.ToString())
            };
        
            await ExecuteNonQueryAsync(INSERT_USERCOURSE, parameters);
            _logger.LogInformation("Enrolled user: {UserId} in course {CourseId}", userId, courseId);
            
            return await GetUserCourse(userId, courseId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to save user course for user {UserId} and course {CourseId}", userId, courseId);
            throw;
        }
    }
    
    public async Task<UserCourseRepoData> GetUserCourse(Guid userId, Guid courseId)
    {
        try
        {
            var parameters = new[]
            {
                CreateParameter("@userID", userId.ToString()),
                CreateParameter("@courseId", courseId.ToString())
            };

            await using var reader = await ExecuteReaderAsync(SELECT_USERCOURSE, parameters);

            if (!await reader.ReadAsync())
            {
                throw new InvalidOperationException($"UserCourse not found for user {userId} and course {courseId}");
            }
        
            return new UserCourseRepoData(
                CourseId: reader.GetGuid(reader.GetOrdinal("CourseID")),
                CourseName: reader.GetString(reader.GetOrdinal("CourseName")),
                Level: reader.GetInt16(reader.GetOrdinal("CourseDifficulty")),
                ProgressPercentage: reader.GetInt32(reader.GetOrdinal("ProgressPercentage")),
                CompletionStatus: reader.GetBoolean(reader.GetOrdinal("CompletionStatus")), 
                CoverImage: await Helper.ReadBlobAsync(reader, reader.GetOrdinal("CoverImage"))
            );
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error mapping UserCourse for user {UserId} and course {CourseId}", userId, courseId);
            throw;
        }
    }
}