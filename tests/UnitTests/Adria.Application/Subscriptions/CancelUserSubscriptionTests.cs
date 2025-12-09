using System;
using System.Threading.Tasks;
using Adria.Application.Users;
using Adria.Domain;
using Adria.Domain.Subscriptions;
using Adria.Domain.Users;
using UnitTests.Mocks;
using Xunit;

namespace UnitTests.Subscriptions
{
    public class CancelUserSubscriptionTests
    {
        private readonly MockUserRepository _userRepository = new();
        private readonly MockUserSubscriptionRepository _userSubscriptionRepository = new();

        [Fact]
        public void CancelSubscription_SetsStatusInactive()
        {
            // Arrange
            var subscription = new Subscription(Guid.NewGuid(), "Test", 10m, "test", false);
            var userSub = new UserSubscription(subscription, DateTime.UtcNow);

            // Act
            userSub.Cancel();

            // Assert
            Assert.Equal("Inactive", userSub.Status);
            Assert.True(userSub.EndDate <= DateTime.UtcNow);
        }


        [Fact]
        public async Task CancelSubscription_ShouldThrow_WhenUserNotFound()
        {
            // Arrange
            var useCase = new CancelUserSubscription(
                _userRepository,
                _userSubscriptionRepository
            );

            // Act & Assert
            await Assert.ThrowsAsync<GenesisException>(() =>
                useCase.Execute(new CancelUserSubscriptionInput(Guid.NewGuid())));
        }

        [Fact]
        public async Task CancelSubscription_ShouldThrow_WhenUserHasNoSubscription()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User(userId, "John", 10m);
            _userRepository.AddUser(user);

            var useCase = new CancelUserSubscription(
                _userRepository,
                _userSubscriptionRepository
            );

            // Act & Assert
            await Assert.ThrowsAsync<GenesisException>(() =>
                useCase.Execute(new CancelUserSubscriptionInput(userId)));
        }
    }
}
