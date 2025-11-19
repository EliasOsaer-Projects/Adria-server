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
        var subscription = new SubscriptionData(Guid.NewGuid(), "Basic", 15.99m, ["test"], true);
        
        var course = new CourseData(
            Id: Guid.NewGuid(),
            Name: "Introduction to Java",
            Level: 1,
            Description: "Learn the basics of Java, including syntax, data types, and OOP principles.",
            Category: "Programming",
            Tags: new List<string> { "Java", "Programming"},
            PointsOnCompletion: 100,
            LearnList:[],
            Duration: 10,
            SubscriptionRequired: false
        );
        
        var userCourse = new UserCourseData(
            CourseId: course.Id,
            CourseName: course.Name,
            Difficulty: course.Level,
            CompletionStatus: false,
            ProgressPercentage: 0
        );
        
        var userCourses = new List<UserCourseData> { userCourse };
        
        var userData = new UserData(
            AdriaId: Guid.NewGuid(),
            UserName: "Nathaniel",
            Wallet: 1700m,
            PointsBalance: 0,
            Streak: 0,
            Subscription: subscription
        );
        var mockQuery = new MockSearchUserCourseByUser();
        var mockLogger = new MockLogger<SearchUserCoursesByUser>();
        var useCase = new SearchUserCoursesByUser(mockQuery, mockLogger);
        var input = new SearchUserCoursesByUserInput(userData.AdriaId);

        mockQuery.AddUser(userCourse);


        // Act
        var result = (await useCase.Execute(input)).ToList();

        // Assert
        Assert.NotEmpty(result);
        Assert.Single(result); 
        
        Assert.Equal("Introduction to Java", userCourses.First().CourseName);
        Assert.Equal(1, userCourses.First().Difficulty);
        
        Assert.False(userCourses.First().CompletionStatus);
        Assert.Equal(0, userCourses.First().ProgressPercentage);
    }
}