using Adria.Application.Contracts.Data;
using Adria.Application.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers;

public sealed class Top10UsersController
{
    public static async Task<Results<Ok<IEnumerable<LeaderboardUser>>, NotFound>> Invoke(
        [FromServices] Top10Users top10Users
    )
    {
        var topUsers = await top10Users.Execute(new Top10UsersInput());

        if (!topUsers.Any())
            return TypedResults.NotFound();

        return TypedResults.Ok(topUsers.Select(ToResponse));
    }

    private static LeaderboardUser ToResponse(LeaderBoardData data)
    {
        return new LeaderboardUser(
            Name: data.Name,
            PointsBalance: data.PointsBalance
        );
    }
    
    public sealed record LeaderboardUser(
        string Name,
        int PointsBalance
    );
}