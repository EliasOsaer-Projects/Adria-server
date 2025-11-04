using Microsoft.Extensions.Logging;
using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;

namespace Adria.Application;

public sealed class AllSubscriptionTypes : IUseCase<Task<IReadOnlyCollection<SubscriptionData>>>
{
    private readonly IAllSubscriptionTypes _subscriptionTypes;
    private readonly ILogger<AllSubscriptionTypes> _logger;

    public AllSubscriptionTypes(IAllSubscriptionTypes subscriptionTypes, ILogger<AllSubscriptionTypes> logger)
    {
        _subscriptionTypes = subscriptionTypes;
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<SubscriptionData>> Execute()
    {
        _logger.LogInformation("Fetching all Subscription types ...");

        return await _subscriptionTypes.Fetch();

    }

}
