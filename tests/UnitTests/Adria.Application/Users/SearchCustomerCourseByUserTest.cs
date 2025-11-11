using Adria.Application.Users;
using UnitTests.Mocks;

namespace UnitTests.Adria.Application.Users;

public sealed class SearchCustomerCourseByUserTest
{
    /*
    [Fact]
    public async Task Execute_ReturnsCustomerCourses()
    {
        // Arrange
        var mockQuery = new MockSearchCustomerCourseByUser();
        var mockLogger = new MockLogger<SearchCustomerCoursesByUser>();
        var useCase = new SearchCustomerCoursesByUser(mockQuery, mockLogger);
        var input = new SearchCustomerCoursesByUserInput();

        for (int i = 1; i <= 15; i++)
        {
            mockQuery.AddUser(new LeaderBoardData(
                Name: $"Testuser-{i}",
                PointsBalance: i
            ));
        }

        // Act
        var result = (await useCase.Execute(input)).ToList();

        // Assert
        Assert.Equal(10, result.Count);
        Assert.Equal("Testuser-15", result[0].Name);
        Assert.Equal("Testuser-6", result[9].Name);
    }
    */
}