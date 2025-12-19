using Adria.Domain.PushNotifications;

namespace UnitTests.Mocks;

public sealed class MockPushNotificationSender : IPushNotificationSender
{
    public List<(PushSubscription Subscription, string Title, string Message, string Url)> SentNotifications { get; } = [];

    public Task SendNotification(PushSubscription pushSubscription, string title, string message, string url)
    {
        SentNotifications.Add((pushSubscription, title, message, url));
        return Task.CompletedTask;
    }
}
