using Microsoft.Extensions.Logging;
using Adria.Application.Contracts.Data;

namespace Adria.Application;

public sealed class AllSubscriptionTypes
{
    private readonly IAllSubscriptionTypes _subscriptionTypes;
    private readonly ILogger<AllSubscriptionTypes> logger;
}
