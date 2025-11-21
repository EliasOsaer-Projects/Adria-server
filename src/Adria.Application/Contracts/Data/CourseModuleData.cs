namespace Adria.Application.Contracts.Data;

public sealed record CourseModuleData
(
    Guid ModuleId,
    string Title,
    string Description,
    int Order
);