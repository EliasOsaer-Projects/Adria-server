using Adria.Application.Contracts;
using Adria.Domain;
using Adria.Domain.Subscriptions;
using Adria.Domain.Users;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Users;

public sealed record SubscribeUserInput(
    Guid AdriaId,
    Guid SubscriptionId,
    string? PromoCode
);

public sealed class SubscribeUser : IUseCase<SubscribeUserInput, Task>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserSubscriptionRepository _userSubscriptionRepository;
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly ILogger<SubscribeUser> _logger;
    private readonly IDiscountByPromoCodeQuery _promoQuery;
    public SubscribeUser(
        IUserRepository userRepository,
        IUserSubscriptionRepository userSubscriptionRepository,
        ISubscriptionRepository subscriptionRepository,
        ILogger<SubscribeUser> logger,
        IDiscountByPromoCodeQuery promoQuery)
    {
        _userRepository = userRepository;
        _userSubscriptionRepository = userSubscriptionRepository;
        _subscriptionRepository = subscriptionRepository;
        _logger = logger;
        _promoQuery = promoQuery;
    }

    public async Task Execute(SubscribeUserInput input)
    {
        User user = await _userRepository.ById(input.AdriaId)
                    ?? throw new ArgumentException($"User {input.AdriaId} not found.");

        Subscription subscription = await _subscriptionRepository.ById(input.SubscriptionId)
                                    ?? throw new ArgumentException($"Subscription {input.SubscriptionId} not found.");

        decimal? discount = null;

        if (!string.IsNullOrWhiteSpace(input.PromoCode))
        {
            discount = await _promoQuery.Fetch(input.PromoCode);
            if (discount is null)
                throw new ArgumentException("Invalid or inactive promo code.");
        }

        UserSubscription? current = await _userSubscriptionRepository.ById(user.AdriaId);

        if (current != null)
        {
            current.ChangeSubscription(subscription, DateTime.UtcNow);
            _logger.LogInformation("User subscription updated: {UserId}", user.AdriaId);
        }
        else
        {
            current = new UserSubscription(subscription, DateTime.UtcNow);
            _logger.LogInformation("User subscribed successfully: {UserId}", user.AdriaId);
        }

        await _userSubscriptionRepository.Save(user, current, input.PromoCode);
    }
}
