
using Adria.Application.Contracts;
using Adria.Application.Courses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi;

public class GetLessonsCountController
{
    private GetLessonsCountController(){}
    public static async Task<Results<Ok<int>, NotFound>> Invoke(
        [FromRoute] Guid courseId,
        [FromServices] IUseCase<GetLessonsCountInput, Task<int>> getLessonsCount
    )
    {
        return TypedResults.Ok(await getLessonsCount.Execute(new GetLessonsCountInput(courseId)));
    }
}