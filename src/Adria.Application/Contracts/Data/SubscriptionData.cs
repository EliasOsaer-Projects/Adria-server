using Adria.Domain;

namespace Adria.Application.Contracts.Data;

public sealed record SubscriptionData(
  string Type,
  decimal PriceCycle,
  string Description
);
