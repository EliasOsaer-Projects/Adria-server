using Adria.Domain;
using Adria.Domain.Contracts;

namespace UnitTests.Mocks
{
    public class MockUserCourseRepository : IUserCourseRepository
    {
        private readonly Dictionary<(Guid userId, Guid courseId), UserCourse> _courses = new();
        public int? LastSavedProgress { get; private set; }
        public bool ReturnNull { get; set; } = false;

        public Task<UserCourse?> ById(Guid userId, Guid courseId)
        {
            if (ReturnNull) return Task.FromResult<UserCourse?>(null);

            if (_courses.TryGetValue((userId, courseId), out var existing))
                return Task.FromResult<UserCourse?>(existing);

            var userCourse = new UserCourse(userId, courseId);
            _courses[(userId, courseId)] = userCourse;
            return Task.FromResult<UserCourse?>(userCourse);
        }

        public Task Save(UserCourse userCourse, int progress)
        {
            LastSavedProgress = progress;
            _courses[(userCourse.AdriaId, userCourse.CourseId)] = userCourse;
            return Task.CompletedTask;
        }

        public Task<UserCourseRepoData> Save(Guid userId, Guid courseId)
        {
            var repoData = new UserCourseRepoData(
                courseId,
                "Mock Course",
                1,
                false,
                Array.Empty<byte>(),
                0
            );

            _courses[(userId, courseId)] = new UserCourse(userId, courseId);
            return Task.FromResult(repoData);
        }
    }
}