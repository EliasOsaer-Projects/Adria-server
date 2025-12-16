using Adria.Application.Courses;
using Adria.Application.Contracts.Data;
using Adria.Domain;
using UnitTests.Mocks;
using Microsoft.Extensions.Logging.Abstractions;

namespace UnitTests.Adria.Application.Courses
{
    public class GetNextCourseModuleTests
    {
        [Fact]
        public async Task Execute_WithExistingModule_ReturnsCourseModule()
        {
            var courseId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var moduleData = new CourseModuleData(courseId, 1, "Module 1", new List<ModuleQuestionData>());

            var mockQuery = new MockGetNextCourseModuleQuery();
            var mocklogger = new MockLogger<GetNextCourseModule>();
            mockQuery.AddModule(courseId, userId, moduleData);

            var useCase = new GetNextCourseModule(mockQuery, mocklogger);
            var input = new GetNextCourseModuleInput(courseId, userId);

            var result = await useCase.Execute(input);

            Assert.NotNull(result);
            Assert.Equal(moduleData, result);
        }

        [Fact]
        public async Task Execute_WhenNoModuleFound_ThrowsGenesisException()
        {
            var courseId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            var mockQuery = new MockGetNextCourseModuleQuery();
            var useCase = new GetNextCourseModule(mockQuery, NullLogger<GetNextCourseModule>.Instance);
            var input = new GetNextCourseModuleInput(courseId, userId);

            var ex = await Assert.ThrowsAsync<GenesisException>(() => useCase.Execute(input));
            Assert.Contains("No CourseModule found", ex.Message);
        }
    }
}