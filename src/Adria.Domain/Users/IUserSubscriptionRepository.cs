namespace Adria.Domain;

public interface IUserSubscriptionRepository
{
    Task<UserSubscription?> ByUserId(Guid adriaId);
    Task Save(UserSubscription userSubscription, Guid adriaId);
}