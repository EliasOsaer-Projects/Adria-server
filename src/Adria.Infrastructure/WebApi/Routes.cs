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
        Title = "INKEA Genesis API",
        Description = "Your even nicer description here",
        Contact = new OpenApiContact
        {
            Name = "Group 01",
            Email = "info@group-01.adria"
        }
    };

    public static WebApplication MapRoutes(this WebApplication app)
    {
        MapUserRoutes(app);
        MapSubscriptionRoutes(app);
        MapCourseRoutes(app);
        MapCosmeticRoutes(app);

        return app;
    }

    private static void MapCourseRoutes(WebApplication app)
    {
        var courseRoutes = app
            .MapGroup("api")
            .WithTags("Courses")
            .WithDescription("All endpoints todo with courses")
            .WithOpenApi();

        courseRoutes.MapGet("/courses", SearchAllCoursesController.Invoke)
            .WithDescription("Get all courses")
            .WithName(nameof(SearchAllCoursesController))
            .WithMetadata(new ProducesAttribute(APPLICATION_JSON))
            .WithOpenApi();

        courseRoutes
            .MapGet("/courses/{courseId}", SearchCourseByIdController.Invoke)
            .WithDescription("Get a certain course by its ID")
            .WithName(nameof(SearchCourseByIdController))
            .WithMetadata(new ProducesAttribute(APPLICATION_JSON))
            .WithOpenApi();

        courseRoutes
            .MapGet("/courses/{courseId}/modules/{userId}", GetNextCourseModuleController.Invoke)
            .WithDescription("Get's the next module for a course for a certain user")
            .WithName(nameof(GetNextCourseModuleController))
            .WithMetadata(new ProducesAttribute(APPLICATION_JSON))
            .WithOpenApi();
        
        courseRoutes
            .MapPost("courses/{courseId}/question/{questionId}/answer/{answerId}/{userId}", CheckAnswerController.Invoke)
            .WithDescription("Checks the user's answer for a question, returns id of correct answer")
            .WithName(nameof(CheckAnswerController))
            .WithMetadata(new ProducesAttribute(APPLICATION_JSON))
            .WithOpenApi();
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
            .MapGet("/{userId}/courses", SearchUserCoursesByUserController.Invoke)
            .WithDescription("Get all CustomerCourses and their corresponding course by user.")
            .WithName(nameof(SearchUserCoursesByUserController))
            .WithMetadata(new ProducesAttribute(APPLICATION_JSON))
            .WithOpenApi();

        userRoutes
            .MapPost("/{userId}/enroll/{courseId}", EnrollUserInCourseController.Invoke)
            .WithDescription("Enroll a user to a certain course")
            .WithName(nameof(EnrollUserInCourseController))
            .WithMetadata(new ProducesAttribute(APPLICATION_JSON))
            .WithOpenApi();

        userRoutes
            .MapPost("/{userId}/subscribe/{subscriptionId}", SubscribeUserToSubscriptionController.Invoke)
            .WithDescription("Subscribe a user to a subscription")
            .WithName(nameof(SubscribeUserToSubscriptionController))
            .WithMetadata(new ConsumesAttribute(APPLICATION_JSON))
            .WithOpenApi();

        userRoutes
            .MapPost("/{userId}/subscription/cancel", CancelUserSubscriptionController.Invoke)
            .WithDescription("cancels a user's subscription")
            .WithName(nameof(CancelUserSubscriptionController))
            .WithMetadata(new ConsumesAttribute(APPLICATION_JSON))
            .WithOpenApi();

        userRoutes
          .MapPatch("/{userId}/userCourses/{courseId}/{progress}", AddProgressToCourseController.Invoke)
          .WithDescription("Adds porgress to the user course")
          .WithName(nameof(AddProgressToCourseController))
          .WithMetadata(new ConsumesAttribute(APPLICATION_JSON))
          .WithOpenApi();

        userRoutes
            .MapPost("/{userId}/profilePicture", UpdateUserProfilePictureController.Invoke)
            .WithDescription("Update a user's profile picture")
            .WithName(nameof(UpdateUserProfilePictureController))
            .WithMetadata(new ConsumesAttribute(APPLICATION_JSON))
            .WithOpenApi();

        userRoutes
            .MapDelete("/{userId}/profilePicture", DeleteUserProfilePictureController.Invoke)
            .WithDescription("Delete a user's profile picture")
            .WithName(nameof(DeleteUserProfilePictureController))
            .WithMetadata(new ProducesAttribute(APPLICATION_JSON))
            .WithOpenApi();
        
        userRoutes
            .MapPost("/{userId}/occupation/{occupation}", AddOccupationController.Invoke)
            .WithDescription("Add the users occupation and interests to the user")
            .WithName(nameof(AddOccupationController))
            .WithMetadata(new ConsumesAttribute(APPLICATION_JSON))
            .WithOpenApi();
        
        userRoutes
            .MapPost("/{userId}/interests/{interests}", AddInterestsController.Invoke)
            .WithDescription("Add the users interests to the user")
            .WithName(nameof(AddInterestsController))
            .WithMetadata(new ConsumesAttribute(APPLICATION_JSON))
            .WithOpenApi();
        
        userRoutes
            .MapPost("/occupations", GetAllOccupationsController.Invoke)
            .WithDescription("Get all possible occupations")
            .WithName(nameof(GetAllOccupationsController))
            .WithMetadata(new ConsumesAttribute(APPLICATION_JSON))
            .WithOpenApi();
        
        userRoutes
            .MapPost("/interests", GetAllInterestsController.Invoke)
            .WithDescription("Get all possible interests")
            .WithName(nameof(GetAllInterestsController))
            .WithMetadata(new ConsumesAttribute(APPLICATION_JSON))
            .WithOpenApi();
    }

    private static void MapSubscriptionRoutes(WebApplication app)
    {

        var subscriptionRoutes = app
            .MapGroup("api/subscriptions")
            .WithTags("Subscriptions")
            .WithDescription("All endpoints todo with subscriptions")
            .WithOpenApi();

        subscriptionRoutes.MapGet("/all-subscriptions", AllSubscriptionTypesController.Invoke).WithDescription("All subscriptions types").
          WithName(nameof(AllSubscriptionTypesController)).WithMetadata(new ProducesAttribute(APPLICATION_JSON)).
          WithOpenApi();

        subscriptionRoutes.MapGet("/{subscriptionId}", SearchSubscriptionTypeByIdController.Invoke)
          .WithDescription("Get the certain subscription type")
          .WithName(nameof(SearchSubscriptionTypeByIdController))
          .WithMetadata(new ProducesAttribute(APPLICATION_JSON))
          .WithOpenApi();
    }

    private static void MapCosmeticRoutes(WebApplication app)
    {

        var cosmeticRoute = app
          .MapGroup("api/cosmetics")
          .WithTags("Cosmetics")
          .WithDescription("All endpoints to cosmetics")
          .WithOpenApi();

        cosmeticRoute.MapGet("/all-cosmetics", AllCosmeticsController.Invoke)
          .WithDescription("Query for all cosmetics")
          .WithName(nameof(AllCosmeticsController))
          .WithMetadata(new ProducesAttribute(APPLICATION_JSON))
          .WithOpenApi();
    }
}
