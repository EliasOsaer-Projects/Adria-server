using Adria.Domain;

namespace Adria.Application.Contracts.Data;

public sealed record UserCourseData
(
    Guid CourseId,
    string CourseName,
    int Difficulty,
    Boolean CompletionStatus,
    int ProgressPercentage
);