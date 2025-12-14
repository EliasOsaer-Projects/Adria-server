using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers;


public sealed class GetAllInterestsController
{
    private GetAllInterestsController(){}
    
    public static async Task<IResult> Invoke(
        [FromServices] IUseCase<Task<IReadOnlyCollection<InterestData>>> getAllInterests)
    {
        var interests = await getAllInterests.Execute();

        var result = interests
            .Select(o => o.InterestName)
            .ToList();
        
        return Results.Ok(result);
    }
}