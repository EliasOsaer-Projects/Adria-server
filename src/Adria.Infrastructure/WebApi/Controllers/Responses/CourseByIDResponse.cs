using Adria.Application.Contracts.Data;

namespace Adria.Infrastructure.WebApi.Controllers.Responses;

public sealed record CourseByIDResponse(
    Guid Id,
    string Name,
    int Level,
    string Description,
    string Category ,
    IList<string> Tags,
    int PointsOnCompletion,
    IList<string> LearnList,
    int Duration,
    byte[] BannerImage,
    Boolean SubscriptionRequired
);