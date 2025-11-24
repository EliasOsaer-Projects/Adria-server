using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Adria.Application.Subscriptions;
using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
namespace Adria.Infrastructure;

public sealed class SearchSubscriptionTypeByIdController
{
    public static async Task<Results<Ok<SubscriptionData>, NotFound>> Invoke(
        Guid subscriptionId,
        [FromServices] IUseCase<SearchSubscriptionTypeByIdInput, Task<SubscriptionData>> searchSubscriptionById
        )
    {
        if (subscriptionId == Guid.Empty)
            return TypedResults.NotFound();

        SearchSubscriptionTypeByIdInput input = new SearchSubscriptionTypeByIdInput(subscriptionId);
        SubscriptionData subscriptionData = await searchSubscriptionById.Execute(input);

        return TypedResults.Ok(subscriptionData);

    }

    private SearchSubscriptionTypeByIdController() { }
}
