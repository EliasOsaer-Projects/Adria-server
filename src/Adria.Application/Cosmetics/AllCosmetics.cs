using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Microsoft.Extensions.Logging;

namespace Adria.Application;

public sealed class AllCosmetics : IUseCase<Task<IReadOnlyCollection<CosmeticItemData>>>
{
    private readonly IAllCosmeticsQuery _allCosmetics;
    private readonly ILogger<AllCosmetics> _logger;

    public AllCosmetics(IAllCosmeticsQuery cosmetics, ILogger<AllCosmetics> logger)
    {
        _allCosmetics = cosmetics;
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<CosmeticItemData>> Execute()
    {
        _logger.LogInformation("Fetching all Cosmetics");

        return await _allCosmetics.Fetch();
    }
}
