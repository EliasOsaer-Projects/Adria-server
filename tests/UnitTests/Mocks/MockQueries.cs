using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;

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

    public Task<IReadOnlyCollection<CourseData>> Fetch(Guid UserId)
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

public class MockCourseModulesByCourseIdQuery : ICourseModulesByCourseIdQuery
{
    private readonly Dictionary<Guid, List<CourseModuleData>> _modulesByCourse = new();

    public void AddModules(Guid courseId, List<CourseModuleData> modules)
    {
        _modulesByCourse[courseId] = modules;
    }

    public Task<IEnumerable<CourseModuleData>> Fetch(Guid courseId)
    {
        _modulesByCourse.TryGetValue(courseId, out var modules);
        return Task.FromResult<IEnumerable<CourseModuleData>>(modules ?? new List<CourseModuleData>());
    }
}

public class MockCurrentPromoCodesQuery : IActivePromoCodesQuery
{
    public IEnumerable<string> PromoCodes { get; set; } = [];

    public Task<IEnumerable<string>> Fetch()
        => Task.FromResult(PromoCodes);
}

public class MockAllOccupationsQuery : IAllOccupationsQuery
{
    public IReadOnlyCollection<OccupationData> Occupations { get; set; } = [];

    public Task<IReadOnlyCollection<OccupationData>> Fetch()
        => Task.FromResult(Occupations);
}

public class MockAllInterestsQuery : IAllInterestsQuery
{
    public IReadOnlyCollection<InterestData> Interests { get; set; } = [];

    public Task<IReadOnlyCollection<InterestData>> Fetch()
        => Task.FromResult(Interests);
}

public class MockDiscountByPromoCodeQuery : IDiscountByPromoCodeQuery
{
    private readonly Dictionary<string, decimal> _promoCodes = new Dictionary<string, decimal>();
    public void AddPromoCode(string promoCode, decimal discount)
    {
        _promoCodes[promoCode] = discount;
    }

    public Task<decimal> Fetch(string promoCode)
    {
        if (_promoCodes.TryGetValue(promoCode, out var discount))
        {
            return Task.FromResult(discount);
        }

        return Task.FromResult(0m);
    }
}

public class MockAllCosmeticsQuery : IAllCosmeticsQuery
{
    private readonly List<CosmeticItemData> _cosmeticItems = new();

    public Task<IReadOnlyCollection<CosmeticItemData>> Fetch(Guid userId)
    {
        return Task.FromResult((IReadOnlyCollection<CosmeticItemData>)_cosmeticItems);
    }

    public void AddCosmeticItem(CosmeticItemData item)
    {
        _cosmeticItems.Add(item);
    }
}

public class MockGetNextCourseModuleQuery : IGetNextCourseModuleQuery
{
    private readonly Dictionary<(Guid courseId, Guid userId), CourseModuleData> _modules = new();

    public Task<CourseModuleData?> Fetch(Guid courseId, Guid userId)
    {
        _modules.TryGetValue((courseId, userId), out var module);
        return Task.FromResult(module);
    }

    public void AddModule(Guid courseId, Guid userId, CourseModuleData module)
    {
        _modules[(courseId, userId)] = module;
    }
}