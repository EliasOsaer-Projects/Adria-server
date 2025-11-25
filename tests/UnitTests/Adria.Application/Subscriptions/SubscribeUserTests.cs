using Adria.Application.Subscriptions;
using Adria.Application.Users;
using Adria.Domain;
using Adria.Domain.Users;
using UnitTests.Mocks;

namespace UnitTests.Adria.Application.Subscriptions;

public sealed class SubscribeUserTests
{
    [Fact]
    public async Task Execute_WithValidInput_CreatesSubscriptionForNewUser()
    {
        // Arrange
        var mockUserRepo = new MockUserRepository();
        var mockSubscriptionRepo = new MockSubscriptionRepository();
        var mockUserSubscriptionRepo = new MockUserSubscriptionRepository();
        var mockLogger = new MockLogger<SubscribeUser>();

        var user = new User(Guid.NewGuid(), "Alice", 100);
        var subscription = new Subscription(Guid.NewGuid(), "Premium", 50, "Best plan", true);

        mockUserRepo.AddUser(user);
        mockSubscriptionRepo.AddSubscription(subscription);

        var useCase = new SubscribeUser(
            mockUserRepo, 
            mockUserSubscriptionRepo, 
            mockSubscriptionRepo, 
            mockLogger
        );

        var input = new SubscribeUserInput(user.AdriaId, subscription.SubscriptionId);

        // Act
        await useCase.Execute(input);

        // Assert
        var savedSubscription = await mockUserSubscriptionRepo.ById(user.AdriaId);
        Assert.NotNull(savedSubscription);
        Assert.Equal(subscription.SubscriptionId, savedSubscription!.CurrentSubscription.SubscriptionId);
        Assert.True(savedSubscription.Status);
        Assert.Single(mockLogger.LoggedMessages);
        Assert.Contains("User subscribed successfully", mockLogger.LoggedMessages[0]);
    }

    [Fact]
    public async Task Execute_WithExistingSubscription_UpdatesSubscription()
    {
        // Arrange
        var mockUserRepo = new MockUserRepository();
        var mockSubscriptionRepo = new MockSubscriptionRepository();
        var mockUserSubscriptionRepo = new MockUserSubscriptionRepository();
        var mockLogger = new MockLogger<SubscribeUser>();

        var user = new User(Guid.NewGuid(), "Bob", 100);
        var oldSubscription = new Subscription(Guid.NewGuid(), "Basic", 20, "Old plan", false);
        var newSubscription = new Subscription(Guid.NewGuid(), "Premium", 50, "Best plan", true);

        mockUserRepo.AddUser(user);
        mockSubscriptionRepo.AddSubscription(oldSubscription);
        mockSubscriptionRepo.AddSubscription(newSubscription);

        var existingUserSubscription = new UserSubscription(oldSubscription, DateTime.UtcNow.AddDays(-30));
        await mockUserSubscriptionRepo.Save(user, existingUserSubscription);

        var useCase = new SubscribeUser(
            mockUserRepo, 
            mockUserSubscriptionRepo, 
            mockSubscriptionRepo, 
            mockLogger
        );

        var input = new SubscribeUserInput(user.AdriaId, newSubscription.SubscriptionId);

        // Act
        await useCase.Execute(input);

        // Assert
        var updatedSubscription = await mockUserSubscriptionRepo.ById(user.AdriaId);
        Assert.NotNull(updatedSubscription);
        Assert.Equal(newSubscription.SubscriptionId, updatedSubscription!.CurrentSubscription.SubscriptionId);
        Assert.True(updatedSubscription.Status);
        Assert.True(updatedSubscription.EndDate > DateTime.UtcNow);
        Assert.Single(mockLogger.LoggedMessages);
        Assert.Contains("User subscription updated", mockLogger.LoggedMessages[0]);
    }

    [Fact]
    public async Task Execute_WithInvalidUserId_ThrowsArgumentException()
    {
        // Arrange
        var mockUserRepo = new MockUserRepository();
        var mockSubscriptionRepo = new MockSubscriptionRepository();
        var mockUserSubscriptionRepo = new MockUserSubscriptionRepository();
        var mockLogger = new MockLogger<SubscribeUser>();

        var subscription = new Subscription(Guid.NewGuid(), "Premium", 50, "Best plan", true);
        mockSubscriptionRepo.AddSubscription(subscription);

        var useCase = new SubscribeUser(
            mockUserRepo, 
            mockUserSubscriptionRepo, 
            mockSubscriptionRepo, 
            mockLogger
        );

        var input = new SubscribeUserInput(Guid.NewGuid(), subscription.SubscriptionId);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => useCase.Execute(input));
    }

    [Fact]
    public async Task Execute_WithInvalidSubscriptionId_ThrowsArgumentException()
    {
        // Arrange
        var mockUserRepo = new MockUserRepository();
        var mockSubscriptionRepo = new MockSubscriptionRepository();
        var mockUserSubscriptionRepo = new MockUserSubscriptionRepository();
        var mockLogger = new MockLogger<SubscribeUser>();

        var user = new User(Guid.NewGuid(), "Charlie", 100);
        mockUserRepo.AddUser(user);

        var useCase = new SubscribeUser(
            mockUserRepo, 
            mockUserSubscriptionRepo, 
            mockSubscriptionRepo, 
            mockLogger
        );

        var input = new SubscribeUserInput(user.AdriaId, Guid.NewGuid());

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => useCase.Execute(input));
    }
}
