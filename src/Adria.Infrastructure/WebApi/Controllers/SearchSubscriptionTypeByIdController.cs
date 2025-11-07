using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Adria.Application.Subscriptions;
using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
namespace Adria.Infrastructure;

public sealed class SearchSubscriptionTypeByIdController
{
    public static async Task<Results<Ok<List<Subscription>>, NotFound>> Invoke(
        [FromQuery] Guid subscriptionId,
        [FromServices] IUseCase<SearchSubscriptionTypeByIdInput, Task<IReadOnlyCollection<SubscriptionData>>> searchSubscriptionById
        )
    {
        if (subscriptionId == Guid.Empty)
            throw new ArgumentException("Your id is empty");
        SearchSubscriptionTypeByIdInput input = new SearchSubscriptionTypeByIdInput(subscriptionId);
        IReadOnlyCollection<SubscriptionData> subscriptionData = await searchSubscriptionById.Execute(input);

        return TypedResults.Ok(ToRespone(subscriptionData));

    }

    private static List<Subscription> ToRespone(IReadOnlyCollection<SubscriptionData> subscriptions)
    {
        return subscriptions.Select(data => new Subscription(
              Type: data.Type,
                PriceCycle: data.PriceCycle,
                Description: data.Description
              )).ToList();
    }
    private SearchSubscriptionTypeByIdController() { }

}
