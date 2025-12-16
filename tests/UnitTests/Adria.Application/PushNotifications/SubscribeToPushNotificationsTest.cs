using Adria.Application.PushNotifications;
using UnitTests.Mocks;

namespace UnitTests.Adria.Application.PushNotifications;

public sealed class SubscribeToPushNotificationsTest
{
    [Fact]
    public async Task Execute_PersistsSubscription()
    {
        var mockRepository = new MockPushSubscriptionRepository();
        var mockLogger = new MockLogger<SubscribeToPushNotifications>();
        var useCase = new SubscribeToPushNotifications(mockLogger, mockRepository);

        await useCase.Execute(new SubscribeToPushNotificationsInput(
            "https://example.com/push",
            "p256dh-key",
            "auth-key"
        ));

        Assert.Single(mockRepository.StoredSubscriptions);
        Assert.Single(mockLogger.LoggedMessages);
    }
}
