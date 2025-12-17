using Adria.Application;
using Adria.Application.Contracts;
using Adria.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers;

public sealed class PurchaseCosmeticController
{
    private PurchaseCosmeticController(){}
    public static async Task<Results<Ok<string>, NotFound<string>>> Invoke(
        [FromRoute] Guid userId,
        [FromRoute] Guid cosmeticId,
        [FromServices] IUseCase<PurchaseCosmeticInput, Task> purchaseCosmetic 
        )
    {
        try
        {
            await purchaseCosmetic.Execute(new PurchaseCosmeticInput(userId, cosmeticId));
            return TypedResults.Ok("Purchase successfull");
        }
        catch (Exception e)
        {
            return TypedResults.NotFound(e.Message);
        }
    }
}