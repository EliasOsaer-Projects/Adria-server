namespace Adria.Domain.Subscriptions;

public interface ISubscriptionRepository
{
    Task<Subscription?> GetById(Guid subscriptionId);
}