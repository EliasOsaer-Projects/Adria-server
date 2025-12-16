using Adria.Domain;

namespace UnitTests.Adria.Domain
{
    public class PointsPaymentTests
    {
        [Fact]
        public void Constructor_WhenUserCanAfford_CreatesPayment()
        {
            var cosmetic = new CosmeticItem(Guid.NewGuid(), "test item", "border", 100);
            var user = new User(Guid.NewGuid(), "TestUser", 200m);
            user.PointsBalance = 200;

            var payment = new PointsPayment(cosmetic, user);

            Assert.Equal(cosmetic.CosmeticItemID, payment.CosmeticItemID);
            Assert.Equal(user.AdriaId, payment.AdriaID);
            Assert.Equal(cosmetic.Points, payment.PointsAmount);
            Assert.Equal(DateTime.Today, payment.TransactionDate);
            Assert.NotEqual(Guid.Empty, payment.PointsTransactionID);
        }

        [Fact]
        public void Constructor_WhenUserCannotAfford_ThrowsGenesisException()
        {
            var cosmetic = new CosmeticItem(Guid.NewGuid(), "test item", "border", 300);
            var user = new User(Guid.NewGuid(), "TestUser", 200m);
            user.PointsBalance = 200;

            var ex = Assert.Throws<GenesisException>(() => new PointsPayment(cosmetic, user));
            Assert.Equal("User could not afford item.", ex.Message);
        }

        [Fact]
        public void EnsureValidTransaction_StaticMethod_ThrowsWhenNotAffordable()
        {
            var cosmetic = new CosmeticItem(Guid.NewGuid(), "test item", "border", 500);
            var user = new User(Guid.NewGuid(), "TestUser", 400m);
            user.PointsBalance = 400;

            var ex = Assert.Throws<GenesisException>(() =>
                PointsPayment.EnsureValidTransaction(cosmetic, user)
            );

            Assert.Equal("User could not afford item.", ex.Message);
        }

        [Fact]
        public void EnsureValidTransaction_WhenAffordable_AllowsPaymentAndUpdatesBalance()
        {
            var cosmetic = new CosmeticItem(Guid.NewGuid(), "test item", "border", 50);
            var user = new User(Guid.NewGuid(), "TestUser", 100m);
            user.PointsBalance = 100;

            var payment = new PointsPayment(cosmetic, user);
            user.PointsBalance -= (int)payment.PointsAmount;

            Assert.Equal(50, user.PointsBalance);
        }
    }
}