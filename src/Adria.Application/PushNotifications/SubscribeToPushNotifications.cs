using Adria.Application.Contracts;
using Adria.Domain.PushNotifications;
using Microsoft.Extensions.Logging;

namespace Adria.Application.PushNotifications;

public sealed record SubscribeToPushNotificationsInput(string Endpoint, string P256dh, string Auth);

public sealed class SubscribeToPushNotifications
    : IUseCase<SubscribeToPushNotificationsInput, Task>
{
    private readonly ILogger<SubscribeToPushNotifications> _logger;
    private readonly IPushSubscriptionRepository _pushSubscriptionRepository;

    public SubscribeToPushNotifications(
        ILogger<SubscribeToPushNotifications> logger,
        IPushSubscriptionRepository pushSubscriptionRepository
    )
    {
        _logger = logger;
        _pushSubscriptionRepository = pushSubscriptionRepository;
    }

    public async Task Execute(SubscribeToPushNotificationsInput input)
    {
        var subscription = new PushSubscription(input.Endpoint, input.P256dh, input.Auth);

        await _pushSubscriptionRepository.SaveSubscription(subscription);

        _logger.LogInformation("Registered push subscription for endpoint {Endpoint}", subscription.Endpoint);
    }
}
