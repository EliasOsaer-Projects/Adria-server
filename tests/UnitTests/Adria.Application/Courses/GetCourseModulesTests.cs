using Adria.Application.Courses;
using Adria.Application.Contracts.Data;
using UnitTests.Mocks;

namespace UnitTests.Adria.Application.Courses
{
    public class GetCourseModulesTests
    {
        [Fact]
        public async Task Execute_WithExistingCourse_ReturnsModulesWithQuestionsAndAnswers()
        {
            // Arrange
            var courseId = Guid.NewGuid();

            // Create mock modules
            var question1 = new ModuleQuestionData(
                Guid.NewGuid(),
                "What is 2+2?",
                new List<QuestionAnswerData>
                {
                    new QuestionAnswerData(Guid.NewGuid(), "3"),
                    new QuestionAnswerData(Guid.NewGuid(), "4")
                }
            );

            var module1 = new CourseModuleData(
                Guid.NewGuid(),
                1,
                "Introduction Content",
                new List<ModuleQuestionData> { question1 }
            );

            var mockQuery = new MockCourseModulesByCourseIdQuery();
            mockQuery.AddModules(courseId, new List<CourseModuleData> { module1 });

            var mockLogger = new MockLogger<GetCourseModules>();
            var useCase = new GetCourseModules(mockQuery, mockLogger);

            var input = new GetCourseModulesInput(courseId);

            // Act
            var result = await useCase.Execute(input);

            // Assert
            var modules = Assert.Single(result);
            Assert.Equal(module1.ModuleId, modules.ModuleId);
            Assert.Equal(1, modules.Position);
            Assert.Equal("Introduction Content", modules.Content);

            var questions = Assert.Single(modules.Questions);
            Assert.Equal("What is 2+2?", questions.Question);

            var answers = questions.Answers;
            Assert.Contains(answers, a => a.Answer == "4");
            Assert.Contains(answers, a => a.Answer == "3");
        }
    }
}
