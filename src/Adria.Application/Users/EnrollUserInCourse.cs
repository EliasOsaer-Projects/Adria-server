using Adria.Application.Contracts;
using Adria.Domain;
using Adria.Domain.Contracts;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Users;

public sealed record EnrollUserInCourseInput(Guid UserId, Guid CourseId);

public class EnrollUserInCourse 
    : IUseCase<EnrollUserInCourseInput, Task<UserCourseData>>
{
   private readonly ILogger<EnrollUserInCourse> _logger;
    private readonly IUserCourseRepository _userCourseRepository;
    
    public EnrollUserInCourse(
        ILogger<EnrollUserInCourse> logger,
        IUserCourseRepository userCourseRepository
    )
    {
        _logger = logger;
        _userCourseRepository = userCourseRepository;
    }

    public async Task<UserCourseData> Execute(EnrollUserInCourseInput input)
    {
        _logger.LogInformation("Enrolling user {UserId} in course {CourseId}", input.UserId, input.CourseId);
        
        return await _userCourseRepository.Save(input.UserId, input.CourseId);
    }
}