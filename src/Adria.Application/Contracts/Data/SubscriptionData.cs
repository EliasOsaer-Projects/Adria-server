using Adria.Domain;

namespace Adria.Application;

public sealed record SubscriptionData(
  string Type,
  decimal PriceCycle,
  string Description
);
