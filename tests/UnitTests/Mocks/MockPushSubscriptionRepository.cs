using Adria.Domain.PushNotifications;

namespace UnitTests.Mocks;

public sealed class MockPushSubscriptionRepository : IPushSubscriptionRepository
{
    public List<PushSubscription> StoredSubscriptions { get; } = [];

    public IReadOnlyList<PushSubscription> GetSubscriptions()
    {
        return StoredSubscriptions;
    }

    public Task SaveSubscription(PushSubscription pushSubscription)
    {
        StoredSubscriptions.RemoveAll(subscription => subscription.Endpoint == pushSubscription.Endpoint);
        
        StoredSubscriptions.Add(pushSubscription);
        return Task.CompletedTask;
    }
}
