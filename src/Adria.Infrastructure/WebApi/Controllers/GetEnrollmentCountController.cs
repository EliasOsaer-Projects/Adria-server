using Adria.Application.Contracts;
using Adria.Application.Courses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers;

public class GetEnrollmentCountController
{
    private GetEnrollmentCountController(){}

    public static async Task<Results<Ok<int>, NotFound>> Invoke(
        [FromRoute] Guid courseId,
        [FromServices] IUseCase<GetEnrollmentCountInput, Task<int>> getEnrollmentCount
    )
    {
        return TypedResults.Ok(await getEnrollmentCount.Execute(new GetEnrollmentCountInput(courseId)));
    }
}