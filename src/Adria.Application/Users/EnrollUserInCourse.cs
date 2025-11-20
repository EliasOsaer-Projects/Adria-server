using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Users;

public sealed record EnrollUserInCourseInput(Guid UserId, Guid CourseId);

public class EnrollUserInCourse 
    : IUseCase<EnrollUserInCourseInput, Task<UserCourseData>>
{
    private readonly IEnrollUserInCourseQuery _enrollUserInCourseQuery;
    private readonly ILogger<EnrollUserInCourse> _logger;
    
    public EnrollUserInCourse(
        IEnrollUserInCourseQuery enrollUserInCourseQuery, 
        ILogger<EnrollUserInCourse> logger
    )
    {
        _enrollUserInCourseQuery = enrollUserInCourseQuery;
        _logger = logger;
    }

    public async Task<UserCourseData> Execute(EnrollUserInCourseInput input)
    {
        _logger.LogInformation("Enrolling user {UserId} in course {CourseId}", input.UserId, input.CourseId);
        
        return await _enrollUserInCourseQuery.Fetch(input.UserId, input.CourseId);
    }
}