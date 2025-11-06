using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using Adria.Infrastructure.WebApi;

namespace Adria.Infrastructure.WebApi;

public static class Routes
{
    private readonly static string APPLICATION_JSON = "application/json";
    public static OpenApiInfo OpenApiInfo { get; } = new OpenApiInfo
    {
        Version = "v1",
        Title = "Your nice title here",
        Description = "Your even nicer description here",
        Contact = new OpenApiContact
        {
            Name = "Group XX",
            Email = "info@group-XX.adria"
        }
    };

    public static WebApplication MapRoutes(this WebApplication app)
    {
        MapSubscriptionRoutes(app);
        return app;
    }

    private static void MapSubscriptionRoutes(WebApplication app)
    {

        var subscriptionRoutes = app.MapGroup("api/subscriptions").WithTags("subscriptions").WithDescription("All endpoints todo with subscriptions").WithOpenApi();

        subscriptionRoutes.MapGet("/all-subscriptions", AllSubscriptionTypesController.Invoke).WithDescription("All subscriptions types").
          WithName(nameof(AllSubscriptionTypesController)).WithMetadata(new ProducesAttribute(APPLICATION_JSON)).
          WithOpenApi();

        subscriptionRoutes.MapGet("/{subscriptionId}", SearchSubscriptionTypeByIdController.Invoke)
          .WithDescription("Get the certain subscription type")
          .WithName(nameof(SearchSubscriptionTypeByIdController))
          .WithMetadata(new ProducesAttribute(APPLICATION_JSON))
          .WithOpenApi();
    }
}
