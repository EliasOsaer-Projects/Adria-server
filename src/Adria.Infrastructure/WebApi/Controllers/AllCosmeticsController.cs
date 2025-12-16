using Adria.Application;
using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Adria.Infrastructure;

public class AllCosmeticsController
{
    public static async Task<Results<Ok<List<CosmeticItemData>>, NotFound>> Invoke(
        [FromRoute] Guid userId,
        [FromServices] IUseCase<Task<IReadOnlyCollection<CosmeticItemData>>> AllCosmetics
    )
    {
        IReadOnlyCollection<CosmeticItemData> cosmetics = await AllCosmetics.Execute();

        if (cosmetics == null)
            return TypedResults.NotFound();

        return TypedResults.Ok(cosmetics.ToList());
    }
    private AllCosmeticsController() { }
}
