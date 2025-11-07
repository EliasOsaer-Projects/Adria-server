using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Application.Subscriptions;

namespace UnitTests.Mocks;

public class MockUserByIdQuery : IUserByIdQuery
{
    private readonly Dictionary<Guid, UserData> _users = new();

    public Task<UserData?> Fetch(Guid userId)
    {
        _users.TryGetValue(userId, out UserData? user);
        return Task.FromResult(user);
    }

    public void AddUser(UserData user)
    {
        _users[user.Id] = user;
    }

    public void clear()
    {
        _users.Clear();
    }
}

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

public class MockTopUsersByPointsQuery : ITopUsersByPointsQuery
{
    private readonly List<LeaderBoardData> _users = new();

    public void AddUser(LeaderBoardData user) => _users.Add(user);

    public Task<IEnumerable<LeaderBoardData>> Fetch()
    {
        var result = _users.OrderByDescending(u => u.PointsBalance).Take(10);
        return Task.FromResult<IEnumerable<LeaderBoardData>>(result);
    }
}