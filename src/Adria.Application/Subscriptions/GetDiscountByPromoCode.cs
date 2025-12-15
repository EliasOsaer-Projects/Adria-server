using Adria.Application.Contracts;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Subscriptions;

public sealed record GetDiscountByPromoCodeInput(
    string Code
);


public sealed class GetDiscountByPromoCode : IUseCase<GetDiscountByPromoCodeInput, Task<decimal>>
{
    private readonly IDiscountByPromoCodeQuery _DiscountByPromoCode;
    private readonly ILogger<GetActivePromoCodes> _logger;

    public GetDiscountByPromoCode(IDiscountByPromoCodeQuery query, ILogger<GetActivePromoCodes> logger)
    {
        _DiscountByPromoCode = query;
        _logger = logger;
    }

    public async Task<decimal> Execute(GetDiscountByPromoCodeInput input)
    {
        _logger.LogInformation("Checking if the promocode is correct and Fetching the discount");
        return await _DiscountByPromoCode.Fetch(input.Code);
    }

}