using Adria.Domain;
using Adria.Domain.Contracts;

namespace UnitTests.Mocks;

public class MockUserCourseRepository
: IUserCourseRepository
{
    private readonly Dictionary<Guid, User> _users = new();

    public Task<UserCourseRepoData> Save(Guid userId, Guid courseId)
    {
        return Task.FromResult(new UserCourseRepoData(
            courseId,
            "Mock Course",
            1,
            false,
            Array.Empty<byte>(),
            0
        ));
    }
}