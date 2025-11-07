using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Adria.Application.Subscriptions;
using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;


namespace Adria.Infrastructure;

public sealed class AllSubscriptionTypesController
{
    public static async Task<Results<Ok<List<SubscriptionResponse>>, NotFound>> Invoke(
        [FromServices] IUseCase<Task<IReadOnlyCollection<SubscriptionData>>> AllSubscriptionTypes
    )
    {
        IReadOnlyCollection<SubscriptionData> subscriptionDatas = await AllSubscriptionTypes.Execute();

        return TypedResults.Ok(ToResponse(subscriptionDatas));

    }

    private static List<SubscriptionResponse> ToResponse(IReadOnlyCollection<SubscriptionData> subscriptionData)
    {
        return subscriptionData.Select(data => new SubscriptionResponse(
              Type: data.Type,
              PriceCycle: data.PriceCycle
        )).ToList();
    }

    private AllSubscriptionTypesController() { }


}
