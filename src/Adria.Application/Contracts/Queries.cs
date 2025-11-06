using Adria.Application.Contracts.Data;

namespace Adria.Application.Contracts;

public interface IAllSubscriptionTypes
{
    Task<IReadOnlyCollection<SubscriptionData>> Fetch();
}


public interface IAllCoursesQuery
{
    Task<IReadOnlyCollection<CourseData>> Fetch();
}