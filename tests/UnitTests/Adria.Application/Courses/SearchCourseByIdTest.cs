using Adria.Application.Contracts.Data;
using Adria.Application.Courses;
using UnitTests.Mocks;

namespace UnitTests.Courses;

public class SearchCourseByIdTest
{
    [Fact]
    public async Task Execute_ReturnsCourseById()
    {
        // Arrange
        var courseId1 = Guid.NewGuid();
        var courseId2 = Guid.NewGuid();

        var course1 = new CourseData(
            courseId1,
            "Introduction to C#",
            2,
            "Learn the basics of C#, including syntax, data types, and OOP principles.",
            "Programming",
            new List<string>{"C#", "Programming", ".NET", "Beginner"},
            120,
            new List<string>{"variables", "loops", "classes"},
            28,
            CoverImage: new byte[3],
            BannerImage: new byte[3],
            false
        );
        
        var course2 = new CourseData(
            courseId2,
            "Introduction to Java",
            2,
            "Learn the basics of Java, including syntax, data types, and OOP principles.",
            "Programming",
            new List<string>{"Java", "Programming", "Beginner"},
            120,
            new List<string>{"variables", "loops", "classes"},
            28,
            CoverImage: new byte[3],
            BannerImage: new byte[3],
            false
        );
        
        var mockQuery = new MockSearchCourseByIdQuery();
        var mockLogger = new MockLogger<SearchCourseById>();
        var useCase = new SearchCourseById(mockQuery, mockLogger);
        
        mockQuery.AddCourse(course1);
        mockQuery.AddCourse(course2);
        
        // Act
        var result = await useCase.Execute(new SearchCourseByIdInput(courseId1));
        
        // Assert
        Assert.Equal(course1, result);
        Assert.Single(mockLogger.LoggedMessages);
        Assert.Contains($"Fetching course with ID: {courseId1}", mockLogger.LoggedMessages[0]);
    }

}