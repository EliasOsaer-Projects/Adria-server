using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Application.Courses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers;

public sealed class GetCourseModulesController
{
    private GetCourseModulesController(){}
    
    public static async Task<Results<Ok<IEnumerable<CourseModuleData>>, NotFound>> Invoke(
        [FromRoute] Guid courseId,
        [FromServices] IUseCase<GetCourseModulesInput, Task<IEnumerable<CourseModuleData>>> getCourseModules
    )
    {
        var modules = await getCourseModules.Execute(new GetCourseModulesInput(courseId));
        if (!modules.Any()) return TypedResults.NotFound();

        return TypedResults.Ok(modules);
    }
}