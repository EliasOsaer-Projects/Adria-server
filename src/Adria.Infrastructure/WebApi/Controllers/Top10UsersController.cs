using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Application.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers;

public sealed class Top10UsersController
{
    public static async Task<Results<Ok<IEnumerable<LeaderBoardData>>, NotFound>> Invoke(
        [FromServices] IUseCase<Top10UsersInput, Task<IEnumerable<LeaderBoardData>>> top10Users
    )
    {
        var leaderboard = await top10Users.Execute(new Top10UsersInput());
        if (leaderboard == null || !leaderboard.Any())
            return TypedResults.NotFound();

        return TypedResults.Ok(leaderboard);
    }
}