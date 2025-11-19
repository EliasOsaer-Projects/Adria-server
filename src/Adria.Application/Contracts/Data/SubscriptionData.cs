using Adria.Domain;

namespace Adria.Application.Contracts.Data;

public sealed record SubscriptionData(
    Guid SubscriptionId,
    string Name,
    decimal Price,
    IList<string> Description,
    bool IsMostPopular
);
