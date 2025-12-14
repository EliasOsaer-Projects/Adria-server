using Adria.Application.Users;
using Adria.Domain;
using Adria.Domain.Shared.Exceptions;
using UnitTests.Mocks;

namespace UnitTests.Adria.Application.Users;

public class AddInterestsTest
{
    [Fact]
    public async Task Execute_SetsUserInterests_OnExistingUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User(
            userId,
            "Test User",
            1000m
        );

        String interests = "fun, family, friends";

        // Act
        var mockRepo = new MockUserRepository();
        var mockLogger = new MockLogger<AddInterests>();
        var useCase = new AddInterests(mockRepo, mockLogger);

        await mockRepo.Save(user);
        await useCase.Execute(new AddInterestsInput(userId, interests));

        // Assert
        var updatedUser = await mockRepo.ById(userId);
        Assert.NotNull(updatedUser);
        Assert.NotNull(updatedUser.Interests);
        Assert.Contains("fun", updatedUser.Interests);
        Assert.Contains("family", updatedUser.Interests);
        Assert.Contains("friends", updatedUser.Interests);
        Assert.Equal(3, updatedUser.Interests.Count);

    }
    
    [Fact]
    public async Task Execute_Throws_WhenUserNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var mockRepo = new MockUserRepository(); // no user saved with missingUserId
        var mockLogger = new MockLogger<AddInterests>();
        var useCase = new AddInterests(mockRepo, mockLogger);
        var interests = "fun, family, friends";
        var input = new AddInterestsInput(userId, interests);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ElementNotFoundException>(() => useCase.Execute(input));
        Assert.Contains($"User {input.AdriaId} not found.", exception.Message);
    }
    
    [Theory]
    [InlineData("fun")]                 // 1 interest
    [InlineData("fun, family")]         // 2 interests
    [InlineData("a,b,c,d")]             // 4 interests
    [InlineData("")]                    // 0 interests (empty string)
    [InlineData(" , , ")]               // 0 meaningful interests
    public async Task Execute_Throws_WhenInterestCountIsNotExactlyThree(string interests)
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User(userId, "Test User", 1000m);
        var mockRepo = new MockUserRepository();
        var mockLogger = new MockLogger<AddInterests>();
        var useCase = new AddInterests(mockRepo, mockLogger);

        await mockRepo.Save(user);

        // Act & Assert
        await Assert.ThrowsAsync<GenesisException>(async () =>
            await useCase.Execute(new AddInterestsInput(userId, interests)));
    }
}