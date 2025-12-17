using Adria.Application;
using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers;

public sealed class GetUserCosmeticsController
{
    private GetUserCosmeticsController(){}
    
    public static async Task<Results<Ok<IReadOnlyCollection<CosmeticItemData>>, NotFound<string>>> Invoke(
        [FromRoute] Guid userId,
        [FromServices] IUseCase<GetUserCosmeticsInput, Task<IReadOnlyCollection<CosmeticItemData>>> getUserCosmetics 
        )
    {
        try
        {
            IReadOnlyCollection<CosmeticItemData> cosmetics = await getUserCosmetics.Execute(new GetUserCosmeticsInput(userId));
            return TypedResults.Ok(cosmetics);
        }
        catch (Exception e)
        {
            return TypedResults.NotFound(e.Message);
        }
    }
}