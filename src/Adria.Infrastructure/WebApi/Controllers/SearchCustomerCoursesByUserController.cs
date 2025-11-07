using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Application.CustomerCourses;
using Adria.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace Adria.Infrastructure.WebApi.Controllers;

public sealed class SearchCustomerCoursesByUserController
{
    public static async Task<Results<Ok<List<User>>, NotFound>> Invoke(
        [FromServices]
        IUseCase<SearchCustomerCoursesByUserInput, Task<IReadOnlyCollection<UserData>>> searchCustomerCoursesByUser,
        [FromRoute] string userId
    )
    {
        IReadOnlyCollection<UserData> UserData =
            await searchCustomerCoursesByUser.Execute(new SearchCustomerCoursesByUserInput(userId));

        if (UserData == null || UserData.Count == 0)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(ToResponse(UserData.ToList()));
    }


    private static List<User> ToResponse(IReadOnlyCollection<UserData> UserData)
    {
        return UserData.Select(data => new User(
                id: data.AdriaId,
                name: data.UserName,
                wallet: data.Wallet,
                courses: (data.CustomerCourses?.Select(customerCourse => new CustomerCourse(
                    id: customerCourse.CustomerCourseId,
                    progress: customerCourse.Progress,
                    completionStatus: customerCourse.CompletionStatus,
                    course: new Course(customerCourse.Course.CourseId, customerCourse.Course.CourseName,
                        customerCourse.Course.Category,
                        customerCourse.Course.Description, customerCourse.Course.Tags,
                        customerCourse.Course.LearnList,
                        customerCourse.Course.Duration, customerCourse.Course.Level, customerCourse.Course.Modules,
                        customerCourse.Course.PointsOnCompletion)
                )).ToList()) ?? []))
            .ToList();
    }
}

   