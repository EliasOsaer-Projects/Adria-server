using Adria.Application.Contracts;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Courses;

public sealed record GetEnrollmentCountInput(Guid CourseId);

public class GetEnrollmentCount
    :IUseCase<GetEnrollmentCountInput, Task<int>>

{
    private readonly IGetEnrollmentCountQuery _query;
    private readonly ILogger<GetEnrollmentCount> _logger;

    public GetEnrollmentCount(IGetEnrollmentCountQuery query, ILogger<GetEnrollmentCount> logger)
    {
        _query = query;
        _logger = logger;
    }

    public async Task<int> Execute(GetEnrollmentCountInput input)
    {
        _logger.LogInformation("Fetching enrollment count for course {CourseId}", input.CourseId);
        return await _query.Fetch(input.CourseId);
    }
}