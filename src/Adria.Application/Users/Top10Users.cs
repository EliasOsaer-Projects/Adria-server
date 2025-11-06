using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Users;

public sealed record Top10UsersInput();

public class Top10Users : IUseCase<Top10UsersInput, Task<IEnumerable<UserData>>>
{
    private readonly ITopUsersByPointsQuery _query;
    private readonly ILogger<Top10Users> _logger;

    public Top10Users(ITopUsersByPointsQuery query, ILogger<Top10Users> logger)
    {
        _query = query;
        _logger = logger;
    }

    public async Task<IEnumerable<UserData>> Execute(Top10UsersInput input)
    {
        _logger.LogInformation("Finding Top 10 users");
        return await _query.Fetch();
    }
}