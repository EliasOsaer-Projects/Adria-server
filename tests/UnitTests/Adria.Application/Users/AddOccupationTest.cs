using Adria.Application.Users;
using Adria.Domain;
using Adria.Domain.Shared.Exceptions;
using UnitTests.Mocks;

namespace UnitTests.Adria.Application.Users;

public class AddOccupationTest
{
    [Fact]
    public async Task Execute_SetsUserOccupation_OnExistingUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User(
            userId,
            "Test User",
            1000m
        );

        String occupation = "cop";

        // Act
        var mockRepo = new MockUserRepository();
        var mockLogger = new MockLogger<AddOccupation>();
        var useCase = new AddOccupation(mockRepo, mockLogger);

        await mockRepo.Save(user);
        await useCase.Execute(new AddOccupationInput(userId, occupation));

        // Assert
        var updatedUser = await mockRepo.ById(userId);
        Assert.NotNull(updatedUser);
        Assert.NotNull(updatedUser.Occupation);
        Assert.Contains("cop", updatedUser.Occupation);
    }
    
    [Fact]
    public async Task Execute_Throws_WhenUserNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var mockRepo = new MockUserRepository(); 
        var mockLogger = new MockLogger<AddInterests>();
        var useCase = new AddInterests(mockRepo, mockLogger);
        String occupation = "cop";
        var input = new AddInterestsInput(userId, occupation);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ElementNotFoundException>(() => useCase.Execute(input));
        Assert.Contains($"User {input.AdriaId} not found.", exception.Message);
    }
    
    
    [Theory]
    [InlineData("")]                    // empty occupation
    [InlineData("3")]                  // number
    public async Task Execute_Throws_WhenOccupationIsNotCorrect(string interests)
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