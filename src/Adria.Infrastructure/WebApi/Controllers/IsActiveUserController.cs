using Adria.Application.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers;

public sealed class IsActiveUserController
{
    private IsActiveUserController() { }
    public static async Task<Results<Ok<bool>, NotFound>> Invoke(
        [FromRoute] Guid userId,
        [FromServices] IUseCase<Guid, Task<bool>> isActiveUser
    )
    {
        var isActive = await isActiveUser.Execute(userId);
        return TypedResults.Ok(isActive);
    }
    
}