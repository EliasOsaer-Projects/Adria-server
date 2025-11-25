using Adria.Domain;

namespace Adria.Application.Contracts.Data;

public sealed record CourseData(
    Guid Id,
    string Name,
    int Level,
    string Description,
    string Category ,
    IList<string> Tags,
    int PointsOnCompletion,
    IList<string> LearnList,
    int Duration,
    byte[] CoverImage,
    byte[] BannerImage,
    Boolean SubscriptionRequired
);
