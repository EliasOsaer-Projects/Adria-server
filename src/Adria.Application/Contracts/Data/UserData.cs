using Adria.Domain;

namespace Adria.Application.Contracts.Data;

public sealed record UserData
(
    Guid Id,
    string Name,
    decimal Wallet,
    int PointsBalance,
    DateTime LastPayment,
    int Streak,
    SubscriptionData? Subscription
);