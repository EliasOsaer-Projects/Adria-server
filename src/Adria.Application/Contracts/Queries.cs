using Adria.Application.Contracts.Data;

namespace Adria.Application.Contracts;

public interface IUserByIdQuery
{
    Task<UserData?> Fetch(Guid userId);
}

public interface IAllSubscriptionTypes
{
    Task<IReadOnlyCollection<SubscriptionData>> Fetch();
}

public interface ISearchSubscriptionTypeByIdQuery
{
    Task<SubscriptionData> Fetch(Guid id);
}

public interface ITopUsersByPointsQuery
{
    Task<IEnumerable<LeaderBoardData>> Fetch();
}

public interface IAllCoursesQuery
{
    Task<IReadOnlyCollection<CourseData>> Fetch();
}

public interface IUserCoursesByUserQuery
{
    Task<IReadOnlyCollection<UserCourseData>> Fetch(Guid id);
}

public interface ISearchCourseByIdQuery
{
    Task<CourseData> Fetch(Guid id);
}

public interface ICourseModulesByCourseIdQuery
{
    Task<IEnumerable<CourseModuleData>> Fetch(Guid courseId);
}

public interface IAllCosmeticsQuery
{
    Task<IReadOnlyCollection<CosmeticItemData>> Fetch();
}
