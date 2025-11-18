using Adria.Infrastructure.WebApi.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;

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
        MapUserRoutes(app);
        MapSubscriptionRoutes(app);
        MapCourseRoutes(app);
        
        return app;
    }

    private static void MapCourseRoutes(WebApplication app)
    {
        var courseRoutes = app.MapGroup("api").WithTags("courses").WithDescription("All endpoints todo with courses").WithOpenApi();

        courseRoutes.MapGet("/courses", SearchAllCoursesController.Invoke).WithDescription("All courses").
            WithName(nameof(SearchAllCoursesController)).WithMetadata(new ProducesAttribute(APPLICATION_JSON)).
            WithOpenApi();
    }

    private static void MapUserRoutes(WebApplication app)
    {
        var userRoutes = app
            .MapGroup("/api/users")
            .WithTags("Users")
            .WithDescription("All endpoints related to Genesis users.")
            .WithOpenApi();
        
        userRoutes
            .MapGet("/{userId}", SearchUserByIdController.Invoke)
            .WithDescription("Get a user by their ID.")
            .WithName(nameof(SearchUserByIdController))
            .WithMetadata(new ProducesAttribute(APPLICATION_JSON))
            .WithOpenApi();
        
        userRoutes
            .MapGet("/leaderboard", Top10UsersController.Invoke)
            .WithDescription("Get the top 10 users with the most points")
            .WithName(nameof(Top10UsersController))
            .WithMetadata(new ProducesAttribute(APPLICATION_JSON))
            .WithOpenApi();
        
        userRoutes
            .MapGet("/{userId}/CustomerCourses", SearchUserCoursesByUserController.Invoke)
            .WithDescription("Get all CustomerCourses and their corresponding course by user.")
            .WithName(nameof(SearchUserCoursesByUserController))
            .WithMetadata(new ProducesAttribute(APPLICATION_JSON))
            .WithOpenApi();
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
