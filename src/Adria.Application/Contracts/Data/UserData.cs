using Adria.Domain;

namespace Adria.Application.Contracts.Data;

public sealed record UserData
(
    string AdriaId,
    string UserName,
    decimal Wallet,
    int PointsBalance,
    int Streak,
    SubscriptionData? Subscription
);