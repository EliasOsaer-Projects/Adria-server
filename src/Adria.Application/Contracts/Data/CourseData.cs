using Adria.Domain;

namespace Adria.Application.Contracts.Data;

public sealed record CourseData(
    Guid Id,
    string Name,
    string Category,
    string Description,
    List<string>? LearnList,
    int Duration,
    string Level,
    List<CourseModule>? Modules,
    int PointsOnCompletion
);
