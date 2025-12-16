using System.Text.Json;
using Adria.Domain.PushNotifications;
using Lib.Net.Http.WebPush;
using Microsoft.Extensions.Logging;

namespace Adria.Infrastructure.PushNotifications;

public sealed class PushNotificationSender : IPushNotificationSender
{
    private readonly PushServiceClient _pushServiceClient;
    private readonly ILogger<PushNotificationSender> _logger;

    public PushNotificationSender(
        PushServiceClient pushServiceClient,
        ILogger<PushNotificationSender> logger
    )
    {
        _pushServiceClient = pushServiceClient;
        _logger = logger;
    }

    public void SendNotification(Adria.Domain.PushNotifications.PushSubscription pushSubscription, string title, string message, string url)
    {
        if (_pushServiceClient.DefaultAuthentication is null)
        {
            throw new InvalidOperationException(
                "Push notifications are not configured. Provide VAPID keys under PushNotifications settings.");
        }

        var subscription = new Lib.Net.Http.WebPush.PushSubscription();
        subscription.Endpoint = pushSubscription.Endpoint;
        subscription.SetKey(PushEncryptionKeyName.P256DH, pushSubscription.P256dh);
        subscription.SetKey(PushEncryptionKeyName.Auth, pushSubscription.Auth);

        var payload = JsonSerializer.Serialize(new
        {
            title,
            message,
            url
        });

        var pushMessage = new PushMessage(payload)
        {
            Topic = "genesis-course-enrollments",
            Urgency = PushMessageUrgency.Normal
        };

        _pushServiceClient
            .RequestPushMessageDeliveryAsync(subscription, pushMessage)
            .GetAwaiter()
            .GetResult();

        _logger.LogInformation("Sent push notification to {Endpoint}", pushSubscription.Endpoint);
    }
}
