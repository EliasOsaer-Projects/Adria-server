namespace Adria.Application.Contracts;

public interface IAllSubscriptionTypes
{
    Task<IReadOnlyCollection<SubscriptionData>> Fetch();
}
