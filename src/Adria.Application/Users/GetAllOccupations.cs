using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Application.Subscriptions;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Users;


public sealed class GetAllOccupations 
    : IUseCase<Task<IReadOnlyCollection<OccupationData>>>
{
    private readonly IAllOccupationsQuery _query;
    private readonly ILogger<GetAllOccupations> _logger;

    public GetAllOccupations(IAllOccupationsQuery query, ILogger<GetAllOccupations> logger)
    {
        _query = query;
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<OccupationData>> Execute()
    {
        _logger.LogInformation("Fetching possible occupations");
        return await _query.Fetch();
    }
}