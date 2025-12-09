using Adria.Application.Contracts;
using Adria.Application.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers;

public sealed class CancelUserSubscriptionController
{
    public static async Task<IResult> Invoke(
        [FromRoute] Guid userId,
        [FromServices] IUseCase<CancelUserSubscriptionInput, Task> cancelSubscription)
    {
        if (userId == Guid.Empty)
            return Results.BadRequest("UserId cannot be empty.");

        await cancelSubscription.Execute(new CancelUserSubscriptionInput(userId));

        return Results.Ok(new { Message = "Subscription canceled successfully." });
    }

    private CancelUserSubscriptionController() { }
}