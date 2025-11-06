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


public interface IAllCoursesQuery
{
    Task<IReadOnlyCollection<CourseData>> Fetch();
}