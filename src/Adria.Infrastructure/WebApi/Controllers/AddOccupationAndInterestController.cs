using Adria.Application.Contracts;
using Adria.Application.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers;

public class AddOccupationAndInterestController
{
    public static async Task<IResult> Invoke(
        [FromRoute] Guid userId,
        [FromRoute] string? occupation,
        [FromRoute] string? interests,
        [FromServices]  IUseCase<AddOccupationAndInterestsInput, Task> AddOccupationAndInterests
    )
    {
        if (userId == Guid.Empty)
        {
            return Results.BadRequest("UserId cannot be empty.");
        }

        var input = new AddOccupationAndInterestsInput( 
            AdriaId: userId,
            Occupation: occupation, 
            Interests: interests
        );

        await AddOccupationAndInterests.Execute(input);

        return Results.Ok(new { Message = "Occupation and Interests added successfully" });
    }
}