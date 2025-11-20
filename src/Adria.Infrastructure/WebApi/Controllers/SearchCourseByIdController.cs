using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Application.Courses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers;

public sealed class SearchCourseByIdController
{
    public static async Task<Results<Ok<CourseData>, NotFound>> Invoke(
        [FromRoute] Guid courseId,
        [FromServices] IUseCase<SearchCourseByIdInput, Task<CourseData>> searchCourseById
    )
    {
        CourseData courseData = await searchCourseById.Execute(new SearchCourseByIdInput(courseId));
        return TypedResults.Ok(courseData);
    }
}