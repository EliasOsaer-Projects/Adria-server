using Adria.Application.Contracts;
using Adria.Domain;
using Adria.Domain.Users;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Users;

public class DeleteUserProfilePicture
    :IUseCase<Guid, Task>
{
    private readonly ILogger<UpdateUserProfilePicture> _logger;
    private readonly IUserRepository _userRepository;

    public DeleteUserProfilePicture(
        ILogger<UpdateUserProfilePicture> logger,
        IUserRepository userRepository
    )
    {
        _logger = logger;
        _userRepository = userRepository;
    }
    
    public async Task Execute(Guid userId)
    {
        User? user = await _userRepository.ById(userId)
                     ?? throw new ArgumentException($"User {userId} not found.");
        
        user.ProfilePicture = null;
        await _userRepository.Save(user);
    }
}