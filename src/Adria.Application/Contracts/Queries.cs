namespace Adria.Application.Contracts.Data;

public interface IAllSubscriptionTypes
{
    Task<IReadOnlyCollection<SubscriptionData>> Fetch();
}

public interface ISearchSubscriptionTypeByIdQuery
{
    Task<IReadOnlyCollection<SubscriptionData>> Fetch(Guid id);
}
