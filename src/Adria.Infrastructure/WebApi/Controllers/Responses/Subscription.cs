namespace Adria.Infrastructure;

public sealed record Subscription(
  string Type,
  decimal PriceCycle,
  string Description
);
