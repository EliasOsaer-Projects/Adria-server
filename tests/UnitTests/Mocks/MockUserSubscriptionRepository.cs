using Adria.Domain;

namespace UnitTests.Mocks;

public class MockUserSubscriptionRepository
{
    private readonly Dictionary<Guid, UserSubscription> _userSubscriptions = new();

    public Task<UserSubscription?> ById(Guid userId)
    {
        _userSubscriptions.TryGetValue(userId, out var subscription);
        return Task.FromResult(subscription);
    }
    
    public Task Save(User user, UserSubscription userSubscription)
    {
        _userSubscriptions[user.AdriaId] = userSubscription;
        return Task.CompletedTask;
    }

    public void Clear()
    {
        _userSubscriptions.Clear();
    }
}