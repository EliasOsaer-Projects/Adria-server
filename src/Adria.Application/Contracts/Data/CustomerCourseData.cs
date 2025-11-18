using Adria.Domain;

namespace Adria.Application.Contracts.Data;

public sealed record CustomerCourseData
(
    Guid CustomerCourseId,
    CourseData Course,
    Boolean CompletionStatus,
    Decimal Progress
);