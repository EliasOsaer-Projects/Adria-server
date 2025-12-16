using Adria.Application;
using UnitTests.Mocks;

namespace UnitTests.Adria.Application.Courses
{
    public sealed class AddProgressToCourseTests
    {
        [Fact]
        public async Task Execute_WithExistingUserCourse_CompletesSuccessfully()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var courseId = Guid.NewGuid();
            var progress = 75;

            var mockRepo = new MockUserCourseRepository();
            var mockLogger = new MockLogger<AddProgressToCourse>();

            var useCase = new AddProgressToCourse(mockRepo, mockLogger);
            var input = new AddProgressToCourseInput(userId, courseId, progress);

            // Act
            await useCase.Execute(input);

            // Assert
            Assert.Single(mockLogger.LoggedMessages);
            Assert.Contains("changing data", mockLogger.LoggedMessages[0]);
        }
    }
}