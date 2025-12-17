using Adria.Application.Contracts;
using Adria.Application.Users;
using Adria.Domain;
using Adria.Domain.Users;
using Microsoft.Extensions.Logging;

namespace Adria.Application;

public sealed record PurchaseCosmeticInput(
    Guid UserId,
    Guid CosmeticId
);

public class PurchaseCosmetic
    : IUseCase<PurchaseCosmeticInput, Task>
{
    private readonly ICosmeticRepository _cosmeticRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<PurchaseCosmetic> _logger;

    public PurchaseCosmetic(
        ICosmeticRepository cosmeticRepository,
        ILogger<PurchaseCosmetic> logger,
        IUserRepository  userRepository
    )
    {
        _cosmeticRepository = cosmeticRepository;
        _userRepository = userRepository;
        _logger = logger;
    }
    
    public async Task Execute(PurchaseCosmeticInput input)
    {
        User user = await _userRepository.ById(input.UserId)
                     ?? throw new GenesisException($"User {input.UserId} not found.");
        
        CosmeticItem cosmeticItem = await _cosmeticRepository.ById(input.CosmeticId)
                                   ?? throw new GenesisException($"Cosmetic item {input.CosmeticId} not found.");

        await _cosmeticRepository.Save(cosmeticItem, user);
    }
}