using Microsoft.Extensions.Logging;
using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;

namespace Adria.Application;

public sealed record SearchSubscriptionTypeByIdInput(
    Guid id
    );
public sealed class SearchSubscriptionTypeById : IUseCase<SearchSubscriptionTypeByIdInput, Task<IReadOnlyCollection<SubscriptionData>>>
{
    private ILogger _logger;
    private ISearchSubscriptionTypeById _searchSubscriptionTypeById;
    private Guid id;

    public async Task<IReadOnlyCollection<SubscriptionData>> Execute(SearchSubscriptionTypeByIdInput)
    {

    }
}
