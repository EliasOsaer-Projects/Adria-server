using Adria.Domain;

namespace Adria.Application.Contracts.Data;

public sealed record CourseData(
    Guid CourseId,
    string CourseName,
    string Category,
    string Description,
    List<string> Tags,
    List<string> LearnList,
    int Duration,
    int Level,
    List<CourseModule> Modules,
    int PointsOnCompletion
);
