namespace Adria.Infrastructure;

public sealed record Subscription(
    Guid SubscriptionId,
    string Type,
    decimal PriceCycle,
    string Description,
    bool IsMostPopular
);
