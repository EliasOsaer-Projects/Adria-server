using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Application.Subscriptions;

namespace UnitTests.Mocks;

public class MockUserByIdQuery : IUserByIdQuery
{
    private readonly Dictionary<string, UserData> _users = new();

    public Task<UserData?> Fetch(string userId)
    {
        _users.TryGetValue(userId, out UserData? user);
        return Task.FromResult(user);
    }

    public void AddUser(UserData user)
    {
        _users[user.AdriaId] = user;
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

    private IDictionary<Guid, SubscriptionData> _subscriptions = new Dictionary<Guid, SubscriptionData>();

    public void SetSubscriptions(IDictionary<Guid, SubscriptionData> subscriptions)
    {
        _subscriptions = subscriptions;
    }

    public Task<IReadOnlyCollection<SubscriptionData>> Fetch(Guid id)
    {
        if (_subscriptions.TryGetValue(id, out var user))
        {
            return Task.FromResult<IReadOnlyCollection<SubscriptionData>>(
                new List<SubscriptionData> { user }.AsReadOnly()
            );
        }

        return Task.FromResult<IReadOnlyCollection<SubscriptionData>>(
            Array.Empty<SubscriptionData>()
        );
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

public class MockAllCoursesQuery : IAllCoursesQuery
{
    private readonly IList<CourseData> _courses = new List<CourseData>();

    public Task<IReadOnlyCollection<CourseData>> Fetch()
    {
        return Task.FromResult<IReadOnlyCollection<CourseData>>(_courses.AsReadOnly());
    }

    public void AddCourse(CourseData course) => _courses.Add(course);

    public void Clear() => _courses.Clear();
}

public class MockSearchCustomerCourseByUser : ICustomerCoursesByUserQuery
{
    
    private readonly List<UserData> _users = new();

    public void AddUser(UserData user) => _users.Add(user);
    public Task<IReadOnlyCollection<UserData>> Fetch(string id)
    {
        return Task.FromResult<IReadOnlyCollection<UserData>>(_users.AsReadOnly());
    }
}
