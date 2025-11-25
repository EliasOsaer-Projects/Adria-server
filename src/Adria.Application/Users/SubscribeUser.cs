using Adria.Application.Contracts;
using Adria.Domain;
using Adria.Domain.Subscriptions;
using Adria.Domain.Users;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Users;

public sealed record SubscribeUserInput(
    Guid AdriaId,
    Guid SubscriptionId
);

public sealed class SubscribeUser : IUseCase<SubscribeUserInput, Task>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserSubscriptionRepository _userSubscriptionRepository;
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly ILogger<SubscribeUser> _logger;

    public SubscribeUser(
        IUserRepository userRepository,
        IUserSubscriptionRepository userSubscriptionRepository,
        ISubscriptionRepository subscriptionRepository,
        ILogger<SubscribeUser> logger)
    {
        _userRepository = userRepository;
        _userSubscriptionRepository = userSubscriptionRepository;
        _subscriptionRepository = subscriptionRepository;
        _logger = logger;
    }

    public async Task Execute(SubscribeUserInput input)
    {
        User? user = await _userRepository.ById(input.AdriaId)
            ?? throw new ArgumentException($"User {input.AdriaId} not found.");

        Subscription? subscription = await _subscriptionRepository.ById(input.SubscriptionId)
            ?? throw new ArgumentException($"Subscription {input.SubscriptionId} not found.");

        UserSubscription? current = await _userSubscriptionRepository.ById(user.AdriaId);

        if (current != null)
        {
            current.ChangeSubscription(subscription, DateTime.UtcNow);
        }
        else
        {
            current = new UserSubscription(subscription, DateTime.UtcNow);
        }
        
        await _userSubscriptionRepository.Save(user, current);

        _logger.LogInformation("User {UserId} subscribed to {SubscriptionId}", input.AdriaId, input.SubscriptionId);
    }
}
