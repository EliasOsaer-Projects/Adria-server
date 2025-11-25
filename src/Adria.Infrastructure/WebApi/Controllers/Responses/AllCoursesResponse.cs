namespace Adria.Infrastructure.WebApi.Controllers.Responses;

public sealed record AllCoursesResponse(
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
    Boolean SubscriptionRequired
);