using Adria.Domain;
using Adria.Domain.Contracts;

namespace UnitTests.Mocks;

public class MockUserCourseRepository
: IUserCourseRepository
{
    private readonly Dictionary<Guid, User> _users = new();

    //De ywee eerste functions is om de interface blij te maken.
    public Task<UserCourse?> ById(Guid userId, Guid userCourseId)
    {
        return Task.FromResult<UserCourse?>(new UserCourse(userId, userCourseId));
    }

    public async Task Save(UserCourse userCourse, int progress)
    {
        await Task.CompletedTask;
    }


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
