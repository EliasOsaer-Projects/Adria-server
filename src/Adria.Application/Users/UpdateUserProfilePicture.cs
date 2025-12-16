using Adria.Application.Contracts;
using Adria.Domain;
using Adria.Domain.Users;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Users;

public sealed record UpdateUserProfilePictureInput(Guid UserId, byte[] Image);

public class UpdateUserProfilePicture
    :IUseCase<UpdateUserProfilePictureInput, Task>
{
    private readonly ILogger<UpdateUserProfilePicture> _logger;
    private readonly IUserRepository _userRepository;

    public UpdateUserProfilePicture(
        ILogger<UpdateUserProfilePicture> logger,
        IUserRepository userRepository
    )
    {
        _logger = logger;
        _userRepository = userRepository;
    }
    
    public async Task Execute(UpdateUserProfilePictureInput input)
    {
        _logger.LogInformation("Updating user profile picture");
        User? user = await _userRepository.ById(input.UserId)
            ?? throw new ArgumentException($"User {input.UserId} not found.");

        user.ProfilePicture = input.Image;
        await _userRepository.Save(user);
    }
}