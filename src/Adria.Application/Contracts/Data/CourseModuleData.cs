namespace Adria.Application.Contracts.Data;

public sealed record CourseModuleData(
    Guid ModuleId,
    int Position,
    string Content,
    IReadOnlyCollection<ModuleQuestionData> Questions
);