using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Application.Subscriptions;

namespace UnitTests;

public class MockAllSubscriptionQuerie : IAllSubscriptionTypes
{
    private IList<SubscriptionData> subscriptions = new List<SubscriptionData>();

    public Task<IReadOnlyCollection<SubscriptionData>> Fetch()
    {
        return Task.FromResult<IReadOnlyCollection<SubscriptionData>>(subscriptions.AsReadOnly());
    }

    public void SetSubscriptions(IList<SubscriptionData> subscriptions)
    {
        this.subscriptions = subscriptions;
    }

    public void Clear()
    {
        subscriptions.Clear();
    }
}

public class MockSearchSubscriptionByIdQuery : ISearchSubscriptionTypeByIdQuery
{

    private readonly Dictionary<Guid, SubscriptionData> _user = new Dictionary<Guid, SubscriptionData>();
    public Task<IReadOnlyCollection<SubscriptionData>> Fetch(Guid id)
    {
        IList<SubscriptionData> data = new List<SubscriptionData>();

        _user.TryGetValue(id, out SubscriptionData? user);

        data.Add(user);
        return Task.FromResult<IReadOnlyCollection<SubscriptionData>>(data.AsReadOnly());
    }
}
