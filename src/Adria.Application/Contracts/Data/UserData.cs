using Adria.Domain;

namespace Adria.Application.Contracts.Data;

public sealed record UserData
(
    Guid Id,
    string Name,
    Wallet Wallet,
    int PointsBalance,
    DateTime? LastPayment,
    int Streak,
    Subscritpion? Subscription
);