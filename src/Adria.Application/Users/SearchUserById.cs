using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Users;

public sealed record SearchUserByIdInput(Guid Id);

public class SearchUserById : IUseCase<SearchUserByIdInput, Task<UserData>
{
    private readonly IUserByIdQuery _userByIdQuery;
    private readonly ILogger<SearchUserById> _logger;

    public SearchUserById(
        IUserByIdQuery userByIdQuery,
        ILogger<SearchUserById> logger
    )
    {
        _userByIdQuery = userByIdQuery;
        _logger = logger;
    }
}