using Adria.Application.Contracts;
using Adria.Application.Subscriptions;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Users;


public sealed class GetAllOccupations 
    : IUseCase<Task<IReadOnlyCollection<string>>>
{
    private readonly IAllOccupationsQuery _query;
    private readonly ILogger<GetAllOccupations> _logger;

    public GetAllOccupations(IAllOccupationsQuery query, ILogger<GetAllOccupations> logger)
    {
        _query = query;
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<string>> Execute()
    {
        _logger.LogInformation("Fetching possible occupations");
        return await _query.Fetch();
    }
}