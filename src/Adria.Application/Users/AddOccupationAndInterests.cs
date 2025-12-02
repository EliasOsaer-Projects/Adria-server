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

        if (!string.IsNullOrWhiteSpace(input.Occupation))
        {
            user.AddOccupation(input.Occupation);
            await _userRepository.SaveOccupation(user);
            _logger.LogInformation("Occupation added to user {UserId}", user.AdriaId);
        }

        if (!string.IsNullOrWhiteSpace(input.Interests))
        {
            user.AddInterests(input.Interests.Split(',', StringSplitOptions.TrimEntries).ToList());
            await _userRepository.SaveInterests(user);
            _logger.LogInformation("Interests added to user {UserId}", user.AdriaId);
        }
    }
}
