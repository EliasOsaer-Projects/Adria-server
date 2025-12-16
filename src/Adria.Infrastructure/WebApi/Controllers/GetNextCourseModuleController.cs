using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Application.Courses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers;

public class GetNextCourseModuleController
{
    private GetNextCourseModuleController(){}

    public static async Task<Results<Ok<CourseModuleData>, NotFound>> Invoke(
        [FromRoute] Guid courseId,
        [FromRoute] Guid userId,
        [FromServices] IUseCase<GetNextCourseModuleInput, Task<CourseModuleData>> getNextCourseModule
    )
    {
        Console.WriteLine($"courseID={courseId}, userID={userId}");
        CourseModuleData nextModule = await getNextCourseModule.Execute(new GetNextCourseModuleInput(courseId, userId));
        return TypedResults.Ok(nextModule);
    }
}