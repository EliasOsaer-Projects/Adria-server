using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Application.Users;
using Adria.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers;

public sealed class EnrollUserInCourseController
{
    public static async Task<Results<Ok<UserCourseData>, BadRequest<string>>> Invoke(
            Guid userId,
            Guid courseId,
            [FromServices] IUseCase<EnrollUserInCourseInput, Task<UserCourseData>> enrollUserInCourse,
            [FromServices] IUseCase<SearchUserCoursesByUserInput, Task<IReadOnlyCollection<UserCourseData>>> searchUserCoursesByUser
        )
    {
        IReadOnlyCollection<UserCourseData> userCourseData = 
            await searchUserCoursesByUser.Execute(new SearchUserCoursesByUserInput(userId));

        if (userCourseData.Any(uc => uc.CourseId == courseId))
        {
            return TypedResults.BadRequest("User already enrolled in this course");
        }
        
        UserCourseData userCourse = await enrollUserInCourse.Execute(new EnrollUserInCourseInput(userId, courseId));
        
        return TypedResults.Ok(userCourse);
    }
    
    private EnrollUserInCourseController() { }
}