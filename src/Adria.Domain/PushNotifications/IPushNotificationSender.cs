namespace Adria.Domain.PushNotifications;

public interface IPushNotificationSender
{
    void SendNotification(PushSubscription pushSubscription, string title, string message, string url);
}
