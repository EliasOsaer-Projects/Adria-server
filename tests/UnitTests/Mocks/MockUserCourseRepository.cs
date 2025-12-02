using Adria.Domain;
using Adria.Domain.Contracts;

namespace UnitTests.Mocks;

public class MockUserCourseRepository
: IUserCourseRepository
{
    private readonly Dictionary<Guid, User> _users = new();

    public Task<UserCourseData> Save(Guid adriaId, Guid courseId)
    {
        return Task.FromResult(new UserCourseData(
            courseId,
            "Mock Course",
            1,
            false,
            Array.Empty<byte>(),
            0
        ));
    }
}