using Adria.Application.Contracts.Data;
using Adria.Application.Users;
using Adria.Domain;
using Adria.Domain.Shared.Exceptions;
using UnitTests.Mocks;

namespace UnitTests.Adria.Application.Users;

public sealed class SearchUserByIdTests
{
    [Fact]
    public async Task Execute_WithExistingUser_ReturnsUserData()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var subscription = new SubscriptionData(new Guid("test"), "Basic", 15.99m, "test", true);
        var userData = new UserData(userId, "Nathaniel", 1700, 0, DateTime.Today, 0, subscription);
        var mockQuery = new MockUserByIdQuery();
        mockQuery.AddUser(userData);
        var mockLogger = new MockLogger<SearchUserById>();
        var useCase = new SearchUserById(mockQuery, mockLogger);
        var input = new SearchUserByIdInput(userId);

        // Act
        var result = await useCase.Execute(input);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userId, result.Id);
        Assert.Equal("Nathaniel", result.Name);
        Assert.Equal(0, result.PointsBalance);
        Assert.Equal(0, result.Streak);
        Assert.Single(mockLogger.LoggedMessages);
        Assert.Contains($"Fetching user with ID: {userId}", mockLogger.LoggedMessages[0]);
    }

    [Fact]
    public async Task Execute_WithNonExistentUser_ThrowsElementNotFoundException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var mockQuery = new MockUserByIdQuery();
        var mockLogger = new MockLogger<SearchUserById>();
        var useCase = new SearchUserById(mockQuery, mockLogger);
        var input = new SearchUserByIdInput(userId);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ElementNotFoundException>(() => useCase.Execute(input));
        Assert.Contains($"User with Id {userId} not found", exception.Message);
    }

    [Fact]
    public async Task Execute_WithEmptyGuid_ThrowsElementNotFoundException()
    {
        // Arrange
        var userId = Guid.Empty;
        var mockQuery = new MockUserByIdQuery();
        var mockLogger = new MockLogger<SearchUserById>();
        var useCase = new SearchUserById(mockQuery, mockLogger);
        var input = new SearchUserByIdInput(userId);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ElementNotFoundException>(() => useCase.Execute(input));
        Assert.Contains($"User with Id {userId} not found", exception.Message);
    }
}
