namespace Adria.Application.Contracts.Data;

public interface IAllSubscriptionTypes
{
    Task<IReadOnlyCollection<SubscriptionData>> Fetch();
}

public interface ISearchSubscriptionTypeById
{
    Task<IReadOnlyCollection<SubscriptionData>> Fetch();
}
