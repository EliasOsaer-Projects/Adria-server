using Adria.Application.Contracts;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Subscriptions;

public sealed class GetActivePromoCodes 
    : IUseCase<Task<IEnumerable<string>>>
{
    private readonly IActivePromoCodesQuery _query;
    private readonly ILogger<GetActivePromoCodes> _logger;

    public GetActivePromoCodes(IActivePromoCodesQuery query, ILogger<GetActivePromoCodes> logger)
    {
        _query = query;
        _logger = logger;
    }

    public Task<IEnumerable<string>> Execute()
    {
        _logger.LogInformation("Fetching active promo codes");
        return _query.Fetch();
    }
}