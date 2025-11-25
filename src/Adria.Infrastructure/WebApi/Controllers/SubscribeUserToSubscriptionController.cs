using Adria.Application.Contracts;
using Adria.Application.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers
{
    public sealed class SubscribeUserToSubscriptionController
    {
        public static async Task<IResult> Invoke(
            [FromRoute] Guid userId,
            [FromRoute] Guid subscriptionId,
            [FromServices] IUseCase<SubscribeUserInput, Task> subscribeUser
        )
        {
            if (userId == Guid.Empty || subscriptionId == Guid.Empty)
            {
                return Results.BadRequest("UserId and SubscriptionId cannot be empty.");
            }

            var input = new SubscribeUserInput(
                AdriaId: userId,
                SubscriptionId: subscriptionId
            );

            await subscribeUser.Execute(input);

            return Results.Ok(new { Message = "User subscribed successfully." });
        }
        
        private SubscribeUserToSubscriptionController() { }
    }
}