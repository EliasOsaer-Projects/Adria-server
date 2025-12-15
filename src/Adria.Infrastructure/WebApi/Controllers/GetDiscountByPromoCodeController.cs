using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Application.Subscriptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers;


public sealed class GetDiscountByPromoCodeController
{
    private GetDiscountByPromoCodeController(){}
    
    public static async Task<Results<Ok<decimal>, NotFound>> Invoke(
        [FromRoute] string promoCode,
        [FromServices] IUseCase<GetDiscountByPromoCodeInput, Task<decimal>> getDiscountByPromoCode
    )
    {
        decimal discount = await getDiscountByPromoCode.Execute(new GetDiscountByPromoCodeInput(promoCode));
        return TypedResults.Ok(discount);
    }
}