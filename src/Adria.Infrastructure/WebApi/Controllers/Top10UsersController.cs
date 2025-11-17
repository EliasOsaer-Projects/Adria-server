using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Application.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers;

public sealed class Top10UsersController
{
    private Top10UsersController(){}
    public static async Task<Results<Ok<IEnumerable<LeaderBoardData>>, NotFound>> Invoke(
        [FromServices] IUseCase<Task<IEnumerable<LeaderBoardData>>> top10Users
    )
    {
        var leaderboard = await top10Users.Execute();
        return TypedResults.Ok(leaderboard);
    }
}