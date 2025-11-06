using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Adria.Application.Subscriptions;
using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Application.Subscriptions;
namespace Adria.Infrastructure;

public class SearchSubscriptionTypeByIdController
{
    public static async Task<Results<Ok<List<SubscriptionData>>, NotFound>> Invoke(
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

    private static List<SubscriptionData> ToRespone(IReadOnlyCollection<SubscriptionData> subscriptions)
    {
        return subscriptions.Select(data => new SubscriptionData(
              Type: data.Type,
                PriceCycle: data.PriceCycle,
                Description: data.Description
              )).ToList();
    }
    private SearchSubscriptionTypeByIdController() { }

}
