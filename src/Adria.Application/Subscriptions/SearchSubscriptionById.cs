using Microsoft.Extensions.Logging;
using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;

namespace Adria.Application;

public class SearchSubscriptionTypeById : ISearchSubscriptionTypeById<Task<IReadOnlyCollection<SubscriptionData>>>
{

}
