using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Courses;

public sealed record GetCourseModulesInput(Guid CourseId);

public sealed class GetCourseModules : IUseCase<GetCourseModulesInput, Task<IEnumerable<CourseModuleData>>>
{
    private readonly ICourseModulesByCourseIdQuery _query;
    private readonly ILogger _logger;

    public GetCourseModules(ICourseModulesByCourseIdQuery query, ILogger logger)
    {
        _query = query;
        _logger = logger;
    }

    public async Task<IEnumerable<CourseModuleData>> Execute(GetCourseModulesInput input)
    { 
        _logger.LogInformation("Fetching course modules");
        return await _query.Fetch(input.CourseId);
    }
}