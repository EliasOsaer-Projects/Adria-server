using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;

namespace Adria.Infrastructure;

public sealed class AllSubscriptionTypesController
{
    public static async Task<Results<Ok<List<SubscriptionData>>, NotFound>> Invoke(
        [FromServices] IUseCase<Task<IReadOnlyCollection<SubscriptionData>>> AllSubscriptionTypes
    )
    {
        IReadOnlyCollection<SubscriptionData> subscriptionData = await AllSubscriptionTypes.Execute();

        if (!subscriptionData.Any())
        {
            return TypedResults.NotFound();
        }
        
        return TypedResults.Ok(subscriptionData.ToList());
    }

    private AllSubscriptionTypesController() { }


}
