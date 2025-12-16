using Adria.Application.Users;
using Adria.Domain;
    using Adria.Domain.PushNotifications;
using UnitTests.Mocks;

namespace UnitTests.Adria.Application.Users;

public class EnrollUserInCourseTest
{
    [Fact]
    public async Task Execute_ReturnsAllCourses()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var courseId = Guid.NewGuid();
        
        var user = new User(
            userId,
            "Test User",
            1000m
        );

        var course = new Course(
            courseId,
            "Introduction to Java",
            3,
            "Learn the basics of Java, including syntax, data types, and OOP principles.",
            "Programming",
            new List<string>{"Java", "Programming"},
            200,
            new List<string>{"variables", "loops", "classes"},
            20,
            new Guid(),
            new Guid(),
            false
        );
        
        // Act
        var mockRepo = new MockUserCourseRepository();
        var mockLogger = new MockLogger<EnrollUserInCourse>();
        var mockPushRepository = new MockPushSubscriptionRepository();
        var mockPushSender = new MockPushNotificationSender();
        var useCase = new EnrollUserInCourse(mockLogger, mockRepo, mockPushRepository, mockPushSender);
        
        await mockRepo.Save(user.AdriaId, courseId);
        await mockPushRepository.SaveSubscription(new PushSubscription("https://example.com/endpoint", "p256dh-key", "auth-key"));
        var result = await useCase.Execute(new EnrollUserInCourseInput(userId, courseId));

        // Assert
        Assert.NotNull(result);
        Assert.Equal(courseId, result.CourseId);
        Assert.Single(mockLogger.LoggedMessages);
        Assert.Contains($"Enrolling user {userId} in course {courseId}", mockLogger.LoggedMessages[0]);
        Assert.Single(mockPushSender.SentNotifications);
        Assert.Equal("/courses/" + courseId, mockPushSender.SentNotifications[0].Url);
    }
}
