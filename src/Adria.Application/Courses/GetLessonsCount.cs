using Adria.Application.Contracts;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Courses;


public sealed record GetLessonsCountInput(Guid CourseId);

public class GetLessonsCount
    : IUseCase<GetLessonsCountInput, Task<int>>
{
    private readonly IGetLessonsCountQuery _query;
    private readonly ILogger<GetLessonsCount> _logger;

    public GetLessonsCount(IGetLessonsCountQuery query, ILogger<GetLessonsCount> logger)
    {
        _query = query;
        _logger = logger;
    }

    public async Task<int> Execute(GetLessonsCountInput input)
    {
        _logger.LogInformation("Fetching lesson count");
        return await _query.Fetch(input.CourseId);
    }
}
