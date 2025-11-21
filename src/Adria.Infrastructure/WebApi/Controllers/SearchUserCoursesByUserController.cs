using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Application.Users;
using Adria.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace Adria.Infrastructure.WebApi.Controllers;

public sealed class SearchUserCoursesByUserController
{
    public static async Task<Results<Ok<IReadOnlyCollection<UserCourseData>>, NotFound>> Invoke(
        [FromServices]
        IUseCase<SearchUserCoursesByUserInput, Task<IReadOnlyCollection<UserCourseData>>> searchCustomerCoursesByUser,
        [FromRoute] Guid userId
    )
    {
        IReadOnlyCollection<UserCourseData> userCourseData =
            await searchCustomerCoursesByUser.Execute(new SearchUserCoursesByUserInput(userId));

        if (userCourseData == null || userCourseData.Count == 0)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(userCourseData);
    }
    
    private SearchUserCoursesByUserController() { }
}

   