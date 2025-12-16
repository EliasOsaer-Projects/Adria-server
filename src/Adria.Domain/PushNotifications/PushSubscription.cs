namespace Adria.Domain.PushNotifications;

public sealed class PushSubscription
{
    public PushSubscription(string endpoint, string p256dh, string auth)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(endpoint);
        ArgumentException.ThrowIfNullOrWhiteSpace(p256dh);
        ArgumentException.ThrowIfNullOrWhiteSpace(auth);

        Endpoint = endpoint;
        P256dh = p256dh;
        Auth = auth;
    }

    public string Endpoint { get; }

    public string P256dh { get; }

    public string Auth { get; }
}
