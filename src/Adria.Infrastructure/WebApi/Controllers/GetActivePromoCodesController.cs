using Adria.Application.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers;

public sealed class GetActivePromoCodesController
{
    private GetActivePromoCodesController(){}
    
    public static async Task<IResult> Invoke(
        [FromServices] IUseCase<Task<IEnumerable<string>>> getPromoCodes)
    {
        var result = await getPromoCodes.Execute();
        return Results.Ok(result);
    }
}