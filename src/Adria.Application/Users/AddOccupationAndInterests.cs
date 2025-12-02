using Adria.Application.Contracts;
using Adria.Domain;
using Adria.Domain.Users;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Users;


public sealed record AddOccupationAndInterestsInput(
    Guid AdriaId,
    string Occupation,
    string Interests
);

public sealed class AddOccupationAndInterests : IUseCase<AddOccupationAndInterestsInput, Task>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<AddOccupationAndInterests> _logger;

    public AddOccupationAndInterests(
        IUserRepository userRepository,
        ILogger<AddOccupationAndInterests> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task Execute(AddOccupationAndInterestsInput input)
    {
        User user = await _userRepository.ById(input.AdriaId)
                     ?? throw new ArgumentException($"User {input.AdriaId} not found.");
        
        user.AddOccupation(input.Occupation);
        _logger.LogInformation("Occupation added to user {UserId}", user.AdriaId);
        
        await  _userRepository.SaveOccupation(user);
        
    }
}
