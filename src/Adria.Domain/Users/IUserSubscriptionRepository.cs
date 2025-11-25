namespace Adria.Domain.Users;

public interface IUserSubscriptionRepository
{
    Task<UserSubscription?> ById(Guid userId);
    Task Save(User user, UserSubscription userSubscription);
}