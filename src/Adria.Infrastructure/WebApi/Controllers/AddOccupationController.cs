using Adria.Application.Contracts;
using Adria.Application.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers;

public class AddOccupationController
{
    private AddOccupationController(){}
    public static async Task<IResult> Invoke(
        [FromRoute] Guid userId,
        [FromRoute] string? occupation,
        [FromServices]  IUseCase<AddOccupationInput, Task> AddOccupation
    )
    {
        if (userId == Guid.Empty)
        {
            return Results.BadRequest("UserId cannot be empty.");
        }
        
        if (string.IsNullOrWhiteSpace(occupation))
            occupation = string.Empty;
        
        var input = new AddOccupationInput( 
            AdriaId: userId,
            Occupation: occupation
        );

        await AddOccupation.Execute(input);

        return Results.Ok(new { Message = "Occupation added successfully" });
    }
}