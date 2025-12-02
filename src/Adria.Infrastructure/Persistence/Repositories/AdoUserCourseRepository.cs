using Adria.Domain;
using Adria.Infrastructure.Persistence.Shared;
using Taskly.Infrastructure.Persistence.Shared;
using System.Data.Common;
using Microsoft.Extensions.Logging;


namespace Adria.Infrastructure;

public class AdoUserCourseRepository : AbstractAdoRepository, IUserCourseRepository
{
    private readonly ILogger _logger;
    private readonly IUserCourseRepository _userCourse;

    private static readonly string TABLE_USER_COURSE = "UserCourse";
    private static readonly string COL_ID = "CourseID";
    private static readonly string COL_USER_ID = "AdriaID";
    private static readonly string COL_COMPLETION = "CompletionStatus";
    private static readonly string COL_PROGRESS = "ProgressPercentage";
    private static readonly string COL_START_DATE = "StartDate";
    private static readonly string COL_END_DATE = "EndDate";

    public AdoUserCourseRepository(DbProviderFactory factory, string connectionString, ILogger logger, IUserCourseRepository userCourseRepository) : base(factory, connectionString)
    {
        _logger = logger;
        _userCourse = userCourseRepository;
    }

    private static readonly string SELECT_COURSE_BY_USER_AND_COURSE_ID = @$"
      select {COL_ID}, {COL_USER_ID}, {COL_COMPLETION}, {COL_PROGRESS}, {COL_START_DATE}, {COL_END_DATE} from {TABLE_USER_COURSE}
      where {COL_ID} = @courseId and {COL_USER_ID} = @userId";

    private static readonly string UPDATE_COMPLETION = @$"
      update {TABLE_USER_COURSE}
      set {COL_COMPLETION} = 1, {COL_PROGRESS} = 100
      where {COL_ID} = @courseId and {COL_USER_ID} = @userId";

    private static readonly string UPDATE_PROGRESS = @$"
      update {TABLE_USER_COURSE}
      set {COL_PROGRESS} = {COL_PROGRESS} + @amount
      where {COL_ID} = @courseId and {COL_USER_ID} = @userId";

    public async Task Save(UserCourse course, int progress)
    {
        UserCourse futureStatus = course;
        futureStatus.UpdateProgress(progress);
        var query = futureStatus.CompletionStatus == true ? UPDATE_COMPLETION : UPDATE_PROGRESS;

        try
        {
            var parameters = new[]
            {
              CreateParameter("@userId", course.AdriaId),
              CreateParameter("@courseId", course.CourseId),
              CreateParameter("@amount", progress)
            };

            await ExecuteNonQueryAsync(query, parameters);
            _logger.LogInformation("Progress Updated");
        }
        catch (DbException ex)
        {
            _logger.LogError(ex, "Failed to update for course for user {UserId}", course.AdriaId);
            throw new AdriaDatabaseException("Failed to update course.", ex);
        }
    }

}
