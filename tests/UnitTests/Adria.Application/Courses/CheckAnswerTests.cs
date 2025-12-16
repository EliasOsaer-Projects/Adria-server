using System;
using System.Threading.Tasks;
using Xunit;
using Adria.Application.Courses;
using Adria.Domain;
using UnitTests.Mocks;
using Adria.Domain.Shared.Exceptions;

namespace UnitTests.Adria.Application.Courses
{
    public class CheckAnswerTests
    {
        [Fact]
        public async Task Execute_WithValidData_CallsRepositoriesAndReturnsCorrectAnswerId()
        {
            var userId = Guid.NewGuid();
            var questionId = Guid.NewGuid();
            var answerId = Guid.NewGuid();
            var courseId = Guid.NewGuid();

            var mockLogger = new MockLogger<CheckAnswer>();
            var mockUserQARepo = new MockUserQARepository();
            var mockUserCourseRepo = new MockUserCourseRepository();

            var useCase = new CheckAnswer(mockLogger, mockUserQARepo, mockUserCourseRepo);

            var input = new CheckAnswerInput(questionId, answerId, userId, courseId);

            var result = await useCase.Execute(input);

            Assert.Equal(mockUserQARepo.LastSavedAnswerId, result);
            Assert.Single(mockLogger.LoggedMessages);
            Assert.Contains("Checking answer", mockLogger.LoggedMessages[0]);
            Assert.Equal(mockUserQARepo.GetProgressValue, mockUserCourseRepo.LastSavedProgress);
        }

        [Fact]
        public async Task Execute_WhenUserCourseIsNull_ThrowsGenesisException()
        {
            var userId = Guid.NewGuid();
            var questionId = Guid.NewGuid();
            var answerId = Guid.NewGuid();
            var courseId = Guid.NewGuid();

            var mockLogger = new MockLogger<CheckAnswer>();
            var mockUserQARepo = new MockUserQARepository();
            var mockUserCourseRepo = new MockUserCourseRepository { ReturnNull = true };

            var useCase = new CheckAnswer(mockLogger, mockUserQARepo, mockUserCourseRepo);

            var input = new CheckAnswerInput(questionId, answerId, userId, courseId);

            var exception = await Assert.ThrowsAsync<GenesisException>(() => useCase.Execute(input));
            Assert.Contains("UserCourse not found", exception.Message);
        }
    }
}