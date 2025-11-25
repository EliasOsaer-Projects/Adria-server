using Adria.Application.Contracts.Data;
using Adria.Application.Courses;
using UnitTests.Mocks;

namespace UnitTests.Courses;

public sealed class SearchAllCoursesTests
{
    [Fact]
    public async Task Execute_ReturnsAllCourses()
    {
        // Arrange
   
        var courseData =   new CourseData(
            Id: Guid.NewGuid(),
            Name: "Introduction to C#",
            Level: 2,
            Description: "Learn the basics of C#, including syntax, data types, and OOP principles.",
            Category: "Programming",
            Tags: new List<string> { "C#", "Programming", ".NET", "Beginner" },
            PointsOnCompletion: 100,
            LearnList:[],
            Duration: 10,
            CoverImage: new byte[3],
            BannerImage: new byte[3],
            SubscriptionRequired: false
        );
        
        var courseData2 =   new CourseData(
            Id: Guid.NewGuid(),
            Name: "Introduction to Java",
            Level: 1,
            Description: "Learn the basics of Java, including syntax, data types, and OOP principles.",
            Category: "Programming",
            Tags: new List<string> { "Java", "Programming"},
            PointsOnCompletion: 100,
            LearnList: new List<string>() {"variables", "loops", "classes"},
            Duration: 10,
            CoverImage: new byte[3],
            BannerImage: new byte[3],
            SubscriptionRequired: false
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
        Assert.Contains(result, tl => tl.Name == "Introduction to Java");
        Assert.Contains(result, tl => tl.Level == 1);
        Assert.Contains(result, tl => tl.Description == "Learn the basics of Java, including syntax, data types, and OOP principles.");
        Assert.Contains(result, tl => tl.Category == "Programming");
        Assert.Contains(result, tl => tl.Tags.SequenceEqual(new List<string> { "Java", "Programming" }));
        Assert.Contains(result, tl => tl.PointsOnCompletion == 100);
        Assert.Contains(result, tl => tl.LearnList.SequenceEqual(new List<string> { "variables", "loops", "classes" }));
        Assert.Contains(result, tl => tl.Duration == 10);
        Assert.Contains(result, tl => !tl.SubscriptionRequired);
        Assert.Single(mockLogger.LoggedMessages);
        Assert.Contains($"Fetching all courses", mockLogger.LoggedMessages[0]);
    }
}