using Adria.Domain;
using Adria.Domain.Contracts;
using System.Data.Common;


namespace UnitTests.Mocks;

public class MockUserCourseRepository
: IUserCourseRepository
{
    private readonly Dictionary<Guid, User> _users = new();

    public Task<UserCourse?> ById(Guid userId, Guid userCourseId)
    {
        return Task.FromResult(new UserCourse(userId, userCourseId));
    }

    public async Task Save(UserCourse userCourse, int progress)
    {
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
