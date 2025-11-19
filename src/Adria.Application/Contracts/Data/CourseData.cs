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
    // Guid CoverImageId,
    // Guid BannerImageId,
    Boolean SubscriptionRequired
);
