namespace Adria.Domain.Contracts;

sealed public record UserCourseRepoData
(
    Guid CourseId,
    string CourseName,
    int Level,
    Boolean CompletionStatus,
    byte[] CoverImage,
    int ProgressPercentage
);