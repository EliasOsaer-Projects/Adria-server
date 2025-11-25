namespace Adria.Domain.Users;

public interface IUserSubscriptionRepository
{
    Task<UserSubscription?> GetByUserId(Guid userId);
    Task Save(User user, UserSubscription userSubscription);
}