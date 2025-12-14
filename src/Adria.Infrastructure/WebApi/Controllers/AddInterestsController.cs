using Adria.Application.Contracts;
using Adria.Application.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers;

public class AddInterestsController
{
    public static async Task<IResult> Invoke(
        [FromRoute] Guid userId,
        [FromRoute] string? interests,
        [FromServices]  IUseCase<AddInterestsInput, Task> AddInterests
    )
    {
        if (userId == Guid.Empty)
        {
            return Results.BadRequest("UserId cannot be empty.");
        }
        
        if (string.IsNullOrWhiteSpace(interests))
            interests = string.Empty;
        
        
        var input = new AddInterestsInput( 
            AdriaId: userId,
            Interests: interests
        );

        await AddInterests.Execute(input);

        return Results.Ok(new { Message = "Interests added successfully" });
    }
}