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

    public AdoUserCourseRepository(DbProviderFactory factory, string connectionString, ILogger logger, IUserCourseRepository userCourseRepository) : base(factory, connectionString)
    {
        _logger = logger;
        _userCourse = userCourseRepository;
    }

    public async Task<UserCourse?> ById(Guid id, Guid courseId)
    {
        return new UserCourse(Guid.Empty, Guid.Empty);
    }
    public async Task Save(User user, UserCourse course)
    {

    }

}
