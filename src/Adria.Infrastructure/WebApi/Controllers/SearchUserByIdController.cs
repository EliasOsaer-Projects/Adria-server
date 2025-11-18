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
    
    public static async Task<Results<Ok<UserData>, NotFound>> Invoke(
        [FromRoute] string userId,
        [FromServices] IUseCase<SearchUserByIdInput, Task<UserData>> searchUserById
    )
    {
        UserData userData = await searchUserById.Execute(new SearchUserByIdInput(userId));
        return TypedResults.Ok(userData);
    }
}
