using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Courses;

public sealed record SearchCourseByIdInput(Guid CourseId);

public class SearchCourseById
    :IUseCase<SearchCourseByIdInput, Task<CourseData>>
{
    private readonly ISearchCourseByIdQuery _searchCourseById;
    private readonly ILogger<SearchCourseById> _logger;
    
    public SearchCourseById(
        ISearchCourseByIdQuery searchCourseById, 
        ILogger<SearchCourseById> logger
    )
    {
        _searchCourseById = searchCourseById;
        _logger = logger;
    }
    
    public async Task<CourseData> Execute(SearchCourseByIdInput input)
    {
        _logger.LogInformation(
            "Fetching course with ID: {CourseId}",
            input.CourseId
        );
        
        return await _searchCourseById.Fetch(input.CourseId);
    }
}