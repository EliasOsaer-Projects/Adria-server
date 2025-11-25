using Adria.Domain;

namespace Adria.Application.Contracts.Data;

public sealed record UserCourseData
(
    Guid CourseId,
    string CourseName,
    int Level,
    Boolean CompletionStatus,
    int ProgressPercentage
);