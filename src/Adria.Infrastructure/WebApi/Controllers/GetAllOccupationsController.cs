using Adria.Application.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers;


public sealed class GetAllOccupationsController
{
    private GetAllOccupationsController(){}
    
    public static async Task<IResult> Invoke(
        [FromServices] IUseCase<Task<IReadOnlyCollection<string>>> getAllOccupations)
    {
        var result = await getAllOccupations.Execute();
        return Results.Ok(result);
    }
}