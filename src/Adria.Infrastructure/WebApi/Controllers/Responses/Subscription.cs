namespace Adria.Infrastructure;

public sealed record SubscriptionResponse(
    Guid SubscriptionId,
    string Type,
    decimal PriceCycle,
    string Description,
    bool IsMostPopular
);
