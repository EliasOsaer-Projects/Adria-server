using Adria.Application;
using Adria.Application.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace Adria.Infrastructure.WebApi.Controllers;

public class AddProgressToCourseController
{
    private AddProgressToCourseController(){}
    public static async Task<IResult> Invoke(
        [FromRoute] Guid userId,
        [FromRoute] Guid courseId,
        [FromRoute] int progress,
        [FromServices] IUseCase<AddProgressToCourseInput, Task> AddProgressToCourse
        )
    {
        if (userId == Guid.Empty || courseId == Guid.Empty)
            return Results.BadRequest("User and courseId can't be empty");

        var input = new AddProgressToCourseInput(userId: userId, courseId: courseId, progress: progress);

        await AddProgressToCourse.Execute(input);

        return Results.Ok(new { Message = "Progress added" });
    }

}
