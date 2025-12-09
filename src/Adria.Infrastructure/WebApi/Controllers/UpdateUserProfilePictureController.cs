using Adria.Application.Contracts;
using Adria.Application.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers;
public record UpdateProfilePictureRequest(string ProfilePicture);
public sealed class UpdateUserProfilePictureController
{
    private UpdateUserProfilePictureController(){}
    
    public static async Task<Results<Ok<string>, BadRequest>> Invoke(
        Guid userId,
        [FromBody] UpdateProfilePictureRequest  request,
        [FromServices] IUseCase<UpdateUserProfilePictureInput, Task> updateUserProfilePicture
    )
    {
        if (userId == Guid.Empty || string.IsNullOrWhiteSpace(request.ProfilePicture))
        {
            return TypedResults.BadRequest();
        }
        
        byte[] imageBytes = Convert.FromBase64String(request.ProfilePicture);
        await updateUserProfilePicture.Execute(new UpdateUserProfilePictureInput(userId, imageBytes));
        return TypedResults.Ok("Profile picture updated successfully.");
        
    }
}