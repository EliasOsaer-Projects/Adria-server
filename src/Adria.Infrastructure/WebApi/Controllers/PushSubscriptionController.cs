using Adria.Application.Contracts;
using Adria.Application.PushNotifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers;

public static class PushSubscriptionController
{
    public static async Task<IResult> Invoke(
        [FromBody] PushSubscriptionRequest request,
        [FromServices] IUseCase<SubscribeToPushNotificationsInput, Task> subscribeToPushNotifications
    )
    {
        if (request.Keys is null ||
            string.IsNullOrWhiteSpace(request.Endpoint) ||
            string.IsNullOrWhiteSpace(request.Keys.P256dh) ||
            string.IsNullOrWhiteSpace(request.Keys.Auth))
        {
            return Results.BadRequest("Invalid push subscription payload.");
        }

        await subscribeToPushNotifications.Execute(new SubscribeToPushNotificationsInput(
            request.Endpoint,
            request.Keys.P256dh,
            request.Keys.Auth
        ));

        return Results.Ok();
    }
}
