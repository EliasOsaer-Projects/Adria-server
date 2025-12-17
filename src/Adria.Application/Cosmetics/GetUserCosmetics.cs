using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Microsoft.Extensions.Logging;

namespace Adria.Application;

public sealed record GetUserCosmeticsInput(
    Guid UserId
);

public class GetUserCosmetics
    : IUseCase<GetUserCosmeticsInput, Task<IReadOnlyCollection<CosmeticItemData>>>
{
    private readonly ILogger<GetUserCosmetics> _logger;
    private readonly IGetUserCosmeticsQuery _query;


    public GetUserCosmetics(
        ILogger<GetUserCosmetics> logger,
        IGetUserCosmeticsQuery query
    )
    {
        _logger = logger;
        _query = query;
    }
    
    public async Task<IReadOnlyCollection<CosmeticItemData>> Execute(GetUserCosmeticsInput input)
    {
        _logger.LogInformation("Fetching user cosmetics");

        return await _query.Fetch(input.UserId);
    }
}