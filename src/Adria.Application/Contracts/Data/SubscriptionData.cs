using Adria.Domain;

namespace Adria.Application.Contracts.Data;

public sealed record SubscriptionData(
    string SubscriptionId,
    string Type,
    decimal PriceCycle,
    string Description,
    bool IsMostPopular
);
