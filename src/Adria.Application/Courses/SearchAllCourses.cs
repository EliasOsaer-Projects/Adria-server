using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Courses;

public sealed record SearchAllCoursesInput(
    Guid UserId    
);

public sealed class SearchAllCourses : IUseCase<SearchAllCoursesInput, Task<IReadOnlyCollection<CourseData>>>
{
    private readonly IAllCoursesQuery _allCoursesQuery;
    private readonly ILogger<SearchAllCourses> _logger;
    
    public SearchAllCourses(
        IAllCoursesQuery allCoursesQuery,
        ILogger<SearchAllCourses> logger
    )
    {
        _allCoursesQuery = allCoursesQuery;
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<CourseData>> Execute(SearchAllCoursesInput input)
    {
        _logger.LogInformation(
            "Fetching all courses"
        );

        return await _allCoursesQuery.Fetch(input.UserId);

    }
}