namespace Adria.Domain;

public interface ISubscriptionRepository
{
    Task<Subscription?> GetById(Guid subscriptionId);
}