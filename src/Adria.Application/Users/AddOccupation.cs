using Adria.Application.Contracts;
using Adria.Domain;
using Adria.Domain.Shared.Exceptions;
using Adria.Domain.Users;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Users;


public sealed record AddOccupationInput(
    Guid AdriaId,
    string Occupation
);

public sealed class AddOccupation : IUseCase<AddOccupationInput, Task>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<AddOccupation> _logger;

    public AddOccupation(
        IUserRepository userRepository,
        ILogger<AddOccupation> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task Execute(AddOccupationInput input)
    {
        User user = await _userRepository.ById(input.AdriaId)
                     ?? throw new ElementNotFoundException($"User {input.AdriaId} not found.");

        if (!string.IsNullOrWhiteSpace(input.Occupation))
        {
            user.AddOccupation(input.Occupation);
            await _userRepository.SaveOccupation(user);
            _logger.LogInformation("Occupation added to user {UserId}", user.AdriaId);
        }
    }
}
