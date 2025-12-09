using Adria.Application.Contracts;
using Adria.Domain;
using Adria.Domain.Shared.Exceptions;
using Adria.Domain.Users;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Users;


public sealed record AddInterestsInput(
    Guid AdriaId,
    string Interests
);

public sealed class AddInterests : IUseCase<AddInterestsInput, Task>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<AddInterests> _logger;

    public AddInterests(
        IUserRepository userRepository,
        ILogger<AddInterests> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task Execute(AddInterestsInput input)
    {
        User user = await _userRepository.ById(input.AdriaId)
                     ?? throw new ElementNotFoundException($"User {input.AdriaId} not found.");

        if (!string.IsNullOrWhiteSpace(input.Interests))
        {
            user.AddInterests(input.Interests.Split(',', StringSplitOptions.TrimEntries).ToList());
            await _userRepository.SaveInterests(user);
            _logger.LogInformation("Interests added to user {UserId}", user.AdriaId);
        }
        else
        {
            throw new GenesisException("No interests found.");
        }
    }
}
