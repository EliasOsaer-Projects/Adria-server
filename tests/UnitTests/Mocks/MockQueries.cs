using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Application.Subscriptions;
using Adria.Domain;

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
    private List<SubscriptionData> _subscriptions = new List<SubscriptionData>();

    public Task<IReadOnlyCollection<SubscriptionData>> Fetch()
    {
        return Task.FromResult<IReadOnlyCollection<SubscriptionData>>(_subscriptions.AsReadOnly());
    }

    public void SetSubscriptions(List<SubscriptionData> subscriptions)
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

    private Dictionary<Guid, SubscriptionData> _subscriptions = new Dictionary<Guid, SubscriptionData>();

    public void SetSubscriptions(Dictionary<Guid, SubscriptionData> subscriptions)
    {
        _subscriptions = subscriptions;
    }

    public Task<SubscriptionData> Fetch(Guid id)
    {
        if (_subscriptions.TryGetValue(id, out var user))
        {
            return Task.FromResult(
                new SubscriptionData(
                    SubscriptionId: user.SubscriptionId,
                    Name: user.Name,
                    Price: user.Price,
                    Description: user.Description,
                    IsMostPopular: user.IsMostPopular
                )
            );
        }

        return Task.FromResult(
            new SubscriptionData(Guid.Empty, string.Empty, 0, new List<string>(), false)
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
    private readonly List<CourseData> _courses = new List<CourseData>();

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

public class MockSearchCourseByIdQuery : ISearchCourseByIdQuery
{
    private readonly List<CourseData> _courses = new List<CourseData>();
    public Task<CourseData> Fetch(Guid id)
    {
        return Task.FromResult(_courses.First(c => c.Id == id));
    }
    
    public void AddCourse(CourseData course) => _courses.Add(course);
}

public class MockEnrollUserInCourseQuery : IEnrollUserInCourseQuery
{
    private readonly List<UserCourseData> _userCourses = new List<UserCourseData>();
    
    public void Enroll(Course course, Guid userId)
    {
        _userCourses.Add(
            new UserCourseData(
                course.Id,
                course.Name,
                course.Level,
                false,
                0
            ));
    }
    public Task<UserCourseData> Fetch(Guid userId, Guid courseId)
    {
        return Task.FromResult(_userCourses.First(uc => uc.CourseId == courseId));
    }
}
