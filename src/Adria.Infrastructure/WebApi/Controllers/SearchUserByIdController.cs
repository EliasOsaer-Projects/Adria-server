using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Application.Users;
using Adria.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers;

public sealed class SearchUserByIdController
{
    public static async Task<Results<Ok<User>, NotFound>> Invoke(
        [FromRoute] Guid userId,
        [FromServices] IUseCase<SearchUserByIdInput, Task<UserData>> searchUserById
    )
    {
        UserData userData = await searchUserById.Execute(new SearchUserByIdInput(userId));
        return TypedResults.Ok(ToResponse(userData));
    }

    private static User ToResponse(UserData userData)
    {
        Subscription subscription = new Subscription(userData.Subscription.Type,
            userData.Subscription.PriceCycle, userData.Subscription.Description);
        
        return new User(
            id: userData.Id,
            name: userData.Name,
            lastPayment: userData.LastPayment,
            wallet: userData.Wallet,
            subscription: subscription
        );
    }
}