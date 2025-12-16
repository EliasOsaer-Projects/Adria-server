namespace Adria.Infrastructure.WebApi.Controllers;

public sealed record PushSubscriptionRequest
{
    public string Endpoint { get; init; } = string.Empty;

    public PushSubscriptionKeys Keys { get; init; } = new();
}

public sealed record PushSubscriptionKeys
{
    public string P256dh { get; init; } = string.Empty;

    public string Auth { get; init; } = string.Empty;
}
