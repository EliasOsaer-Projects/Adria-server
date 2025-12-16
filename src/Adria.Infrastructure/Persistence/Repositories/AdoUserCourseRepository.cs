using Adria.Domain;
using Adria.Domain.Contracts;
using Adria.Domain.Shared.Helpers;
using Adria.Domain.Users;
using Adria.Infrastructure.Persistence.Shared;
using Taskly.Infrastructure.Persistence.Shared;
using System.Data.Common;
using Microsoft.Extensions.Logging;


namespace Adria.Infrastructure;

public class AdoUserCourseRepository : AbstractAdoRepository, IUserCourseRepository
{
    private readonly ILogger<AdoUserCourseRepository> _logger;

    private static readonly string TABLE_USER_SUBS = "UserCourse";
    private static readonly string COL_SUBS_ID = "CourseID";
    private static readonly string COL_USER_ID = "AdriaID";
    private static readonly string COL_COMPLETION = "CompletionStatus";
    private static readonly string COL_PROGRESS = "ProgressPercentage";
    private static readonly string COL_START_DATE = "StartDate";
    private static readonly string COL_END_DATE = "EndDate";

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

    public AdoUserCourseRepository(DbProviderFactory factory, string connectionString, ILogger<AdoUserCourseRepository> logger) : base(factory, connectionString)
    {
        _logger = logger;
    }


    private static readonly string SELECT_COURSE_BY_USER_AND_COURSE_ID = @$"
      select {COL_SUBS_ID}, {COL_USER_ID}, {COL_COMPLETION}, {COL_PROGRESS}, {COL_START_DATE}, {COL_END_DATE} from {TABLE_USER_SUBS}
      where {COL_SUBS_ID} = @courseId and {COL_USER_ID} = @userId";

    private static readonly string UPDATE_COMPLETION = @$"
      update {TABLE_USER_SUBS}
      set {COL_COMPLETION} = 1, {COL_PROGRESS} = 100
      where {COL_SUBS_ID} = @courseId and {COL_USER_ID} = @userId";

    private static readonly string UPDATE_PROGRESS = @$"
      update {TABLE_USER_SUBS}
      set {COL_PROGRESS} = @amount
      where {COL_SUBS_ID} = @courseId and {COL_USER_ID} = @userId";

    public async Task<UserCourse?> ById(Guid userId, Guid courseId)
    {
        var param = CreateParameter("@userId", userId);
        var param2 = CreateParameter("@courseId", courseId);

        await using var reader = await ExecuteReaderAsync(SELECT_COURSE_BY_USER_AND_COURSE_ID, [param, param2]);

        if (!await reader.ReadAsync())
            return null;
        
        var id = reader.GetGuid(reader.GetOrdinal(COL_USER_ID));
        var chosenCourseId = reader.GetGuid(reader.GetOrdinal(COL_SUBS_ID));
        var completion = reader.GetBoolean(reader.GetOrdinal(COL_COMPLETION));
        var progress = reader.GetInt16(reader.GetOrdinal(COL_PROGRESS));
        var startDate = reader.GetDateTime(reader.GetOrdinal(COL_START_DATE));
        DateTime? endDate = await reader.IsDBNullAsync(reader.GetOrdinal(COL_END_DATE)) ? null : reader.GetDateTime(reader.GetOrdinal(COL_END_DATE));

        UserCourse userCourse = new(id, chosenCourseId, startDate, endDate, progress, completion);

        return userCourse;
    }

    public async Task Save(UserCourse userCourse, int progress)
    {
        Console.WriteLine(userCourse.ProgressPercentage);
        Console.WriteLine(progress);
        UserCourse futureStatus = userCourse;
        futureStatus.UpdateProgress(progress);
        var query = futureStatus.CompletionStatus ? UPDATE_COMPLETION : UPDATE_PROGRESS;

        try
        {
            var parameters = new[]
            {
              CreateParameter("@userId", userCourse.AdriaId),
              CreateParameter("@courseId", userCourse.CourseId),
              CreateParameter("@amount", progress)
            };

            await ExecuteNonQueryAsync(query, parameters);
            _logger.LogInformation("Progress Updated");
        }
        catch (DbException ex)
        {
            _logger.LogError(ex, "Failed to update for course for user {UserId}", userCourse.AdriaId);
            throw new AdriaDatabaseException("Failed to update course.", ex);
        }
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
