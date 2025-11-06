using Adria.Application.Contracts.Data;
using Adria.Application.Users;

namespace Adria.Application.Contracts;

public interface IUserByIdQuery
{
    Task<UserData> Fetch(Guid userId);
}

public interface IAllSubscriptionTypes
{
    Task<IReadOnlyCollection<SubscriptionData>> Fetch();
}

public interface ITopUsersByPointsQuery
{
    Task<IEnumerable<LeaderBoardData>> Fetch();
}