using Adria.Domain;

namespace Adria.Application.Contracts.Data;

public sealed record UserData
(
    string AdriaId,
    string UserName,
    decimal Wallet,
    int PointsBalance,
    DateTime? LastPayment,
    int Streak,
    SubscriptionData? Subscription,
    List<CustomerCourseData>? CustomerCourses
);