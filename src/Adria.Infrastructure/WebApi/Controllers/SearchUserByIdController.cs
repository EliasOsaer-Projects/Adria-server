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
    private SearchUserByIdController(){}
    
    public static async Task<Results<Ok<User>, NotFound>> Invoke(
        [FromRoute] string userId,
        [FromServices] IUseCase<SearchUserByIdInput, Task<UserData>> searchUserById
    )
    {
        UserData userData = await searchUserById.Execute(new SearchUserByIdInput(userId));
        return TypedResults.Ok(ToResponse(userData));
    }

    private static User ToResponse(UserData userData)
    {
        if (userData.LastPayment != null && userData.Subscription != null)
        {
            Subscription subscription = new Subscription(
                userData.Subscription.Type,
                userData.Subscription.PriceCycle
            );

            return new User(
                id: userData.AdriaId,
                name: userData.UserName,
                lastPayment: userData.LastPayment.Value,
                wallet: userData.Wallet,
                subscription: subscription
            );
        }
        else
        {
            return new User(
                id: userData.AdriaId,
                name: userData.UserName,
                wallet: userData.Wallet
            );
        }
    }
}
