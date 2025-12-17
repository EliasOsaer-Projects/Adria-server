using Adria.Application.Contracts;
using Adria.Domain;
using Adria.Domain.Users;

namespace Adria.Application.Users;

public sealed record CancelUserSubscriptionInput(Guid UserId);

public sealed class CancelUserSubscription : IUseCase<CancelUserSubscriptionInput, Task>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserSubscriptionRepository _userSubscriptionRepository;

    public CancelUserSubscription(
        IUserRepository userRepository,
        IUserSubscriptionRepository userSubscriptionRepository)
    {
        _userRepository = userRepository;
        _userSubscriptionRepository = userSubscriptionRepository;
    }

    public async Task Execute(CancelUserSubscriptionInput input)
    {
        var user = await _userRepository.ById(input.UserId);
        if (user is null)
            throw new GenesisException("User not found");

        var userSubscription = await _userSubscriptionRepository.ById(input.UserId);
        if (userSubscription is null)
            throw new GenesisException("User has no active subscription.");

        userSubscription.Cancel();

        await _userSubscriptionRepository.Save(user, userSubscription, null);
    }
}