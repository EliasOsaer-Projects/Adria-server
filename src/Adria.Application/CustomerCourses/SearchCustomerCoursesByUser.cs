using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Application.Users;
using Adria.Domain;
using Adria.Domain.Shared.Exceptions;
using Microsoft.Extensions.Logging;

namespace Adria.Application.CustomerCourses;


public sealed record SearchCustomerCoursesByUserInput(string UserId);

public sealed class SearchCustomerCoursesByUser : IUseCase<SearchCustomerCoursesByUserInput, Task<IReadOnlyCollection<UserData>>>
{
    private readonly ICustomerCoursesByUserQuery _customerCoursesByUserQuery;
    private readonly ILogger<SearchUserById> _logger;

    public SearchCustomerCoursesByUser(
        ICustomerCoursesByUserQuery customerCoursesByUserQuery,
        ILogger<SearchUserById> logger
    )
    {
        _customerCoursesByUserQuery = customerCoursesByUserQuery;
        _logger = logger;
    }
    
    public async Task<IReadOnlyCollection<UserData>> Execute(SearchCustomerCoursesByUserInput input)
    {
        _logger.LogInformation(
            "Fetching CustomerCourses with UserID: {Id}",
            input.UserId
        );

        return (await _customerCoursesByUserQuery.Fetch(input.UserId))
               ?? throw new ElementNotFoundException(
                   $"User with Id {input.UserId} not found."
               );
    }
}