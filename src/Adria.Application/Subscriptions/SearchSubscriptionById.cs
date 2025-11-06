using Microsoft.Extensions.Logging;
using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;

namespace Adria.Application.Subscriptions;

public sealed record SearchSubscriptionTypeByIdInput(
    Guid id
    );
public sealed class SearchSubscriptionTypeById : IUseCase<SearchSubscriptionTypeByIdInput, Task<IReadOnlyCollection<SubscriptionData>>>
{
    private ILogger<SearchSubscriptionTypeById> _logger;
    private ISearchSubscriptionTypeByIdQuery _searchSubscriptionTypeById;

    public SearchSubscriptionTypeById(ISearchSubscriptionTypeByIdQuery searchSubscriptionTypeById, ILogger<SearchSubscriptionTypeById> logger)
    {
        _searchSubscriptionTypeById = searchSubscriptionTypeById;
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<SubscriptionData>> Execute(SearchSubscriptionTypeByIdInput subscritpion)
    {
        //?
        _logger.LogInformation("Retrieving subscription ...");

        return await _searchSubscriptionTypeById.Fetch(subscritpion.id);

    }
}
