namespace Adria.Domain.PushNotifications;

public interface IPushNotificationSender
{
    Task SendNotification(PushSubscription pushSubscription, string title, string message, string url);
}
