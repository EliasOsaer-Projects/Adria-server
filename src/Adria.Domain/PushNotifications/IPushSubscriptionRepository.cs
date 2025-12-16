namespace Adria.Domain.PushNotifications;

public interface IPushSubscriptionRepository
{
    Task SaveSubscription(PushSubscription pushSubscription);
    IReadOnlyList<PushSubscription> GetSubscriptions();
}
