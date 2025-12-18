using Adria.Application.Contracts.Data;
using Adria.Application.Users;
using Adria.Domain;
using UnitTests.Mocks;

namespace UnitTests.Adria.Application.Users;

public sealed class GetTop10UsersTests
    {
        [Fact]
        public async Task Execute_ReturnsTop10UsersOrderedByPointsBalance()
        {
            // Arrange
            var mockQuery = new MockTopUsersByPointsQuery();
            var mockLogger = new MockLogger<Top10Users>();
            var useCase = new Top10Users(mockQuery, mockLogger);

            for (int i = 1; i <= 15; i++)
            {
                mockQuery.AddUser(new LeaderBoardData(
                    UserId: Guid.NewGuid(), 
                    Name: $"Testuser-{i}",
                    PointsBalance: i,
                    []
                ));
            }

            // Act
            var result = (await useCase.Execute()).ToList();

            // Assert
            Assert.Equal(10, result.Count);
            Assert.Equal("Testuser-15", result[0].Name);
            Assert.Equal("Testuser-6", result[9].Name);
        }

        [Fact]
        public async Task Execute_WithFewerThan10Users_ReturnsAllUsers()
        {
            var mockQuery = new MockTopUsersByPointsQuery();
            var mockLogger = new MockLogger<Top10Users>();
            var useCase = new Top10Users(mockQuery, mockLogger);

            for (int i = 1; i <= 5; i++)
            {
                mockQuery.AddUser(new LeaderBoardData(
                    UserId: Guid.NewGuid(),
                    Name: $"Testuser-{i}",
                    PointsBalance: i * 10
                    ,[]
                ));
            }

            var result = (await useCase.Execute()).ToList();

            Assert.Equal(5, result.Count);
            Assert.Equal("Testuser-5", result[0].Name);
            Assert.Equal("Testuser-1", result[4].Name);
        }

        [Fact]
        public async Task Execute_WithNoUsers_ReturnsEmptyList()
        {
            var mockQuery = new MockTopUsersByPointsQuery();
            var mockLogger = new MockLogger<Top10Users>();
            var useCase = new Top10Users(mockQuery, mockLogger);

            var result = await useCase.Execute();

            Assert.Empty(result);
        }
    }