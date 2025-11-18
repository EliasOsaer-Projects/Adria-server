using Adria.Application.Contracts.Data;
using Adria.Application.Users;
using Adria.Domain;
using UnitTests.Mocks;

namespace UnitTests.Adria.Application.Users;

public sealed class SearchCustomerCourseByUserTest
{
    [Fact]
    public async Task Execute_ReturnsCustomerCourses()
    {
        // Arrange
        var userId = "ID-0125";
        var subscription = new SubscriptionData(Guid.NewGuid(), "Basic", 15.99m, "test", true);
        
        var course = new CourseData(
            CourseId: Guid.NewGuid(),
            CourseName: "Introduction to Programming",
            Category: "Technology",
            Description: "A beginner-friendly programming course.",
            Tags: new List<string> { "Coding", "Basics", "Beginner" },
            LearnList: new List<string> { "Variables", "Loops", "Functions" },
            Duration: 120,
            Level: 1,
            Modules: new List<CourseModule>(),
            PointsOnCompletion: 100
        );
        
        var customerCourse = new CustomerCourseData(
            CustomerCourseId: Guid.NewGuid(),
            Course: course,
            CompletionStatus: false,
            Progress: 0m
        );
        
        var customerCourses = new List<CustomerCourseData> { customerCourse };
        
        var userData = new UserData(
            AdriaId: userId,
            UserName: "Nathaniel",
            Wallet: 1700m,
            PointsBalance: 0,
            LastPayment: DateTime.Today,
            Streak: 0,
            Subscription: subscription,
            CustomerCourses: customerCourses
        );
        var mockQuery = new MockSearchCustomerCourseByUser();
        var mockLogger = new MockLogger<SearchCustomerCoursesByUser>();
        var useCase = new SearchCustomerCoursesByUser(mockQuery, mockLogger);
        var input = new SearchCustomerCoursesByUserInput(userId);

        mockQuery.AddUser(userData);


        // Act
        var result = (await useCase.Execute(input)).ToList();

        // Assert
        Assert.NotEmpty(result);
        Assert.Single(result); 

        var courseResult = result.First();
        
        Assert.Equal("Introduction to Programming", courseResult.CustomerCourses.First().Course.CourseName);
        Assert.Equal("Technology", courseResult.CustomerCourses.First().Course.Category);
        Assert.Equal(120, courseResult.CustomerCourses.First().Course.Duration);
        Assert.Equal(1, courseResult.CustomerCourses.First().Course.Level);
        
        Assert.Contains("Coding", courseResult.CustomerCourses.First().Course.Tags);
        Assert.Contains("Beginner", courseResult.CustomerCourses.First().Course.Tags);
        
        Assert.Contains("Variables", courseResult.CustomerCourses.First().Course.LearnList ?? throw new InvalidOperationException());
        Assert.Contains("Functions", courseResult.CustomerCourses.First().Course.LearnList ?? throw new InvalidOperationException());
        
        Assert.False(courseResult.CustomerCourses.First().CompletionStatus);
        Assert.Equal(0m, courseResult.CustomerCourses.First().Progress);


        Assert.Equal(100, courseResult.CustomerCourses.First().Course.PointsOnCompletion);
    }
}