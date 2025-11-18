using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Domain.Shared.Exceptions;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Users;


public sealed record SearchUserCoursesByUserInput(Guid UserId);

public sealed class SearchUserCoursesByUser 
    : IUseCase<SearchUserCoursesByUserInput, Task<IReadOnlyCollection<UserCourseData>>>
{
    private readonly IUserCoursesByUserQuery _userCoursesByUserQuery;
    private readonly ILogger<SearchUserCoursesByUser> _logger;

    public SearchUserCoursesByUser(
        IUserCoursesByUserQuery userCoursesByUserQuery,
        ILogger<SearchUserCoursesByUser> logger
    )
    {
        _userCoursesByUserQuery = userCoursesByUserQuery;
        _logger = logger;
    }
    
    public async Task<IReadOnlyCollection<UserCourseData>> Execute(SearchUserCoursesByUserInput input)
    {
        _logger.LogInformation(
            "Fetching CustomerCourses with UserID: {Id}",
            input.UserId
        );

        return (await _userCoursesByUserQuery.Fetch(input.UserId))
               ?? throw new ElementNotFoundException(
                   $"User with Id {input.UserId} not found."
               );
    }
}