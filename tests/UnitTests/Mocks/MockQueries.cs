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
        _users[user.AdriaId] = user;
    }

    public void clear()
    {
        _users.Clear();
    }
}

public class MockAllSubscriptionQuery : IAllSubscriptionTypes
{
    private IList<SubscriptionData> _subscriptions = new List<SubscriptionData>();

    public Task<IReadOnlyCollection<SubscriptionData>> Fetch()
    {
        return Task.FromResult<IReadOnlyCollection<SubscriptionData>>(_subscriptions.AsReadOnly());
    }

    public void SetSubscriptions(IList<SubscriptionData> subscriptions)
    {
        this._subscriptions = subscriptions;
    }

    public void Clear()
    {
        _subscriptions.Clear();
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

public class MockSearchUserCourseByUser : IUserCoursesByUserQuery
{
    
    private readonly List<UserCourseData> _users = new();

    public void AddUser(UserCourseData user) => _users.Add(user);
    public Task<IReadOnlyCollection<UserCourseData>> Fetch(Guid id)
    {
        return Task.FromResult<IReadOnlyCollection<UserCourseData>>(_users.AsReadOnly());
    }
}
