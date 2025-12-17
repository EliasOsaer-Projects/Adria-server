using Adria.Application.Contracts;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Users;

public sealed class IsActiveUser
    : IUseCase<Guid, Task<bool>>
{
    private readonly ILogger<IsActiveUser> _logger;
    private readonly IIsActiveUserQuery _query;
    
    public IsActiveUser(ILogger<IsActiveUser> logger, IIsActiveUserQuery query)
    {
        _logger = logger;
        _query = query;
    }
    
    public async Task<bool> Execute(Guid input)
    {
        _logger.LogInformation("Checking if user {UserId} is active", input);
        return await _query.Fetch(input);
    }
}