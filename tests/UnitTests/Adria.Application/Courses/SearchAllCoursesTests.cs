using Adria.Application.Contracts.Data;
using Adria.Application.Courses;
using UnitTests.Mocks;

namespace UnitTests.Courses;

public sealed record SearchAllCoursesInput(
    string Category,
    string Tag
    );

public sealed class SearchAllCoursesTests
{
    [Fact]
    public async Task Execute_ReturnsAllCourses()
    {
        // Arrange
   
        var courseData =   new CourseData(
            Id: Guid.NewGuid(),
            Name: "Introduction to C#",
            Category: "Programming",
            Description: "Learn the basics of C#, including syntax, data types, and OOP principles.",
            Tags: new List<string> { "C#", "Programming", ".NET", "Beginner" },
            LearnList:[],
            Duration: 10,
            Level: 1,
            Modules: [],
            PointsOnCompletion: 100
        );
        
        var courseData2 =   new CourseData(
            Id: Guid.NewGuid(),
            Name: "Introduction to Java",
            Category: "Programming",
            Description: "Learn the basics of Java, including syntax, data types, and OOP principles.",
            Tags: new List<string> { "Java", "Programming", ".NET", "Beginner" },
            LearnList:[],
            Duration: 10,
            Level: 1,
            Modules: [],
            PointsOnCompletion: 100
        );
        
        var mockQuery = new MockAllCoursesQuery();
        var mockLogger = new MockLogger<SearchAllCourses>();
        var useCase = new SearchAllCourses(mockQuery, mockLogger);
        
        mockQuery.AddCourse(courseData);
        mockQuery.AddCourse(courseData2);
    

        // Act
        var result = await useCase.Execute();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Contains(result, tl => tl.Name == "Introduction to C#");
        Assert.Contains(result, tl => tl.Name == "Introduction to Java");
        Assert.Single(mockLogger.LoggedMessages);
        Assert.Contains($"Fetching all courses", mockLogger.LoggedMessages[0]);
    }
}