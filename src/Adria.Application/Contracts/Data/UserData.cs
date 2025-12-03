using Adria.Domain;

namespace Adria.Application.Contracts.Data;

public sealed record UserData
(
    Guid AdriaId,
    string UserName,
    decimal Wallet,
    int PointsBalance,
    int Streak,
    SubscriptionData? Subscription,
    string Occupation,
    byte[]? ProfilePicture
);