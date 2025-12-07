using Adria.Application.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers;

public class DeleteUserProfilePictureController
{
    private DeleteUserProfilePictureController(){}
    
    public static async Task<Results<Ok<string>, BadRequest>> Invoke(
        Guid userId,
        [FromServices] IUseCase<Guid, Task> deleteUserProfilePicture
    )
    {
        if (userId == Guid.Empty)
        {
            return TypedResults.BadRequest();
        }
        
        await deleteUserProfilePicture.Execute(userId);
        return TypedResults.Ok("Profile picture deleted successfully.");
    }
}