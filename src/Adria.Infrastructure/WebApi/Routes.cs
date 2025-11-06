using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using Adria.Infrastructure.WebApi;
using Adria.Infrastructure.WebApi.Controllers;

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
        MapCourseRoutes(app);
        return app;
    }

    private static void MapCourseRoutes(WebApplication app)
    {
        var courseRoutes = app.MapGroup("api/Courses").WithTags("courses").WithDescription("All endpoints todo with courses").WithOpenApi();

        courseRoutes.MapGet("/all-courses", SearchAllCoursesController.Invoke).WithDescription("All courses").
            WithName(nameof(SearchAllCoursesController)).WithMetadata(new ProducesAttribute(APPLICATION_JSON)).
            WithOpenApi();
    }

    private static void MapSubscriptionRoutes(WebApplication app)
    {
        var subscriptionRoutes = app.MapGroup("api/subscriptions").WithTags("subscriptions").WithDescription("All endpoints todo with subscriptions").WithOpenApi();

        subscriptionRoutes.MapGet("/all-subscriptions", AllSubscriptionTypesController.Invoke).WithDescription("All subscriptions types").
          WithName(nameof(AllSubscriptionTypesController)).WithMetadata(new ProducesAttribute(APPLICATION_JSON)).
          WithOpenApi();
    }
}
