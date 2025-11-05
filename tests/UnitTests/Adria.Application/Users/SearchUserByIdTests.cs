using Adria.Application.Contracts.Data;
using Adria.Application.Users;
using Adria.Domain;
using UnitTests.Mocks;

namespace UnitTests.Adria.Application.Users;

public sealed class SearchUserByIdTests
{
    [Fact]
    public async Task Execute_WithExistingUser_ReturnsUserData()
    {
        // Arrange
        var userId = Guid.NewGuid();
        Wallet wallet = new Wallet("NathanielWallet", 0, 10);
        var userData = new UserData(userId, "Nathaniel", wallet, 0, DateTime.Today, 0);
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
        Assert.Contains($"Fetching user with ID {userId}", mockLogger.LoggedMessages[0]);
    }
}