using Adria.Application;
using Adria.Application.Contracts.Data;
using UnitTests.Mocks;

namespace UnitTests.Adria.Application.Cosmetics
{
    public class AllCosmeticsTests
    {
        [Fact]
        public async Task Execute_ShouldReturnAllCosmetics()
        {
            // Arrange
            var mockLogger = new MockLogger<AllCosmetics>();
            var mockQuery = new MockAllCosmeticsQuery();

            mockQuery.AddCosmeticItem(new CosmeticItemData(Guid.NewGuid(), "Hat", "Head", 100m));
            mockQuery.AddCosmeticItem(new CosmeticItemData(Guid.NewGuid(), "Gloves", "Hands", 50m));

            var useCase = new AllCosmetics(mockQuery, mockLogger);

            // Act
            var result = await useCase.Execute();
            var list = result.ToList();

            // Assert
            Assert.Equal(2, list.Count);
            Assert.Contains(list, x => x.itemName == "Hat" && x.points == 100m && x.itemType == "Head");
            Assert.Contains(list, x => x.itemName == "Gloves" && x.points == 50m && x.itemType == "Hands");

            Assert.Single(mockLogger.LoggedMessages);
            Assert.Contains("Fetching all Cosmetics", mockLogger.LoggedMessages[0]);
        }

        [Fact]
        public async Task Execute_ShouldReturnEmptyCollection_WhenNoCosmetics()
        {
            // Arrange
            var mockLogger = new MockLogger<AllCosmetics>();
            var mockQuery = new MockAllCosmeticsQuery();

            var useCase = new AllCosmetics(mockQuery, mockLogger);

            // Act
            var result = await useCase.Execute();

            // Assert
            Assert.Empty(result);
        }
    }
}
