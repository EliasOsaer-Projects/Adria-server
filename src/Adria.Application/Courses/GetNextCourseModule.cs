using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Courses;
public sealed record GetNextCourseModuleInput(Guid CourseId, Guid UserId);

public class GetNextCourseModule
    : IUseCase<GetNextCourseModuleInput, Task<CourseModuleData>>
{
    
    private readonly IGetNextCourseModuleQuery _query;
    private readonly ILogger<GetNextCourseModule> _logger;

    public GetNextCourseModule(IGetNextCourseModuleQuery query, ILogger<GetNextCourseModule> logger)
    {
        _query = query;
        _logger = logger;
    }
    
    public async Task<CourseModuleData> Execute(GetNextCourseModuleInput input)
    {
        _logger.LogInformation("Fetching next course modules for CourseId: {CourseId} and UserId: {UserId}", input.CourseId, input.UserId);
        return await _query.Fetch(input.CourseId, input.UserId);
    }
}