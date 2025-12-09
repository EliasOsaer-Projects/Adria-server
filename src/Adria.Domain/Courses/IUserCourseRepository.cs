using Adria.Domain.Contracts;

namespace Adria.Domain;

public interface IUserCourseRepository
{
    Task<UserCourse?> ById(Guid userId, Guid userCourseId);
    Task Save(UserCourse userCourse, int progress);
    Task<UserCourseRepoData> Save(Guid userId, Guid courseId);
}

