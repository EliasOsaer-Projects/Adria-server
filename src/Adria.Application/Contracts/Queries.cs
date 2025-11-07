using Adria.Application.Contracts.Data;

namespace Adria.Application.Contracts;

public interface IUserByIdQuery
{
    Task<UserData> Fetch(Guid userId);
}

public interface IAllSubscriptionTypes
{
    Task<IReadOnlyCollection<SubscriptionData>> Fetch();
}

public interface ISearchSubscriptionTypeByIdQuery
{
    Task<IReadOnlyCollection<SubscriptionData>> Fetch(Guid id);
}

public interface ITopUsersByPointsQuery
{
    Task<IEnumerable<LeaderBoardData>> Fetch();
}

public interface IAllCoursesQuery
{
    Task<IReadOnlyCollection<CourseData>> Fetch();
}