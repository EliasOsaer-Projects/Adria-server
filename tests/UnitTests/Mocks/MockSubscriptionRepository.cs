using Adria.Domain;
using Adria.Domain.Subscriptions;

namespace UnitTests.Mocks;

public class MockSubscriptionRepository : ISubscriptionRepository
{
    private readonly Dictionary<Guid, Subscription> _subscriptions = new ();
    
    public Task<Subscription?> ById(Guid subscriptionId)
    {
        _subscriptions.TryGetValue(subscriptionId, out var subscription);
        return Task.FromResult(subscription);
    }

    public void AddSubscription(Subscription subscription)
    {
        _subscriptions[subscription.SubscriptionId] = subscription;
    }

    public void Clear()
    {
        _subscriptions.Clear();
    }
}