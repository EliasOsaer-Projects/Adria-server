using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Users;

public sealed class GetAllInterests 
    : IUseCase<Task<IReadOnlyCollection<InterestData>>>
{
    private readonly IAllInterestsQuery _query;
    private readonly ILogger<GetAllInterests> _logger;

    public GetAllInterests(IAllInterestsQuery query, ILogger<GetAllInterests> logger)
    {
        _query = query;
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<InterestData>> Execute()
    {
        _logger.LogInformation("Fetching possible interests");
        return await _query.Fetch();
    }
}