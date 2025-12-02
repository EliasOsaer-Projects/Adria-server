using Adria.Domain.Contracts;

namespace Adria.Domain;

public interface IUserCourseRepository
{
    Task<UserCourseData> Save(Guid user, Guid courseID);
}