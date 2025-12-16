using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Domain;
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
        var courseModule = await _query.Fetch(input.CourseId, input.UserId);

        if (courseModule is null)
        {
            _logger.LogWarning("No next course module found for CourseId: {CourseId} and UserId: {UserId}", input.CourseId, input.UserId);
            throw new GenesisException("No CourseModule found");
        }
        
        return courseModule;
    }
}