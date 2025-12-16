using System;
using Xunit;
using Adria.Domain;

namespace UnitTests.Adria.Domain
{
    public class CosmeticItemTests
    {
        [Fact]
        public void Constructor_InitializesProperties()
        {
            var id = Guid.NewGuid();
            var name = "test item";
            var type = "border";
            decimal points = 100;

            var item = new CosmeticItem(id, name, type, points);

            Assert.Equal(id, item.CosmeticItemID);
            Assert.Equal(name, item.ItemName);
            Assert.Equal(type, item.ItemType);
            Assert.Equal(points, item.Points);
        }
    }
}