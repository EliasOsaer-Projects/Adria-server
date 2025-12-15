using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers;


public sealed class GetAllOccupationsController
{
    private GetAllOccupationsController(){}
    
    public static async Task<IResult> Invoke(
        [FromServices] IUseCase<Task<IReadOnlyCollection<OccupationData>>> getAllOccupations)
    {
        var occupations = await getAllOccupations.Execute();

        var result = occupations
            .Select(o => o.OccupationName)
            .ToList();
        
        return Results.Ok(result);
    }
}