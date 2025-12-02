using Adria.Domain.Contracts;

namespace Adria.Domain;

public interface IUserCourseRepository
{
    Task<UserCourseRepoData> Save(Guid userId, Guid courseId);
}