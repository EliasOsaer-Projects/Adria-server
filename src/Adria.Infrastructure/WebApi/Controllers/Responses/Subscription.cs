namespace Adria.Infrastructure;

public sealed record SubscriptionResponse(
  string Type,
  decimal PriceCycle,
  string Description
);
