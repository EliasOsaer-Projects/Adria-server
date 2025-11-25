namespace Adria.Domain.Subscriptions;

public interface ISubscriptionRepository
{
    Task<Subscription?> ById(Guid subscriptionId);
}