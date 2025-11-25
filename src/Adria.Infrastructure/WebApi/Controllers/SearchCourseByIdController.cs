using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Application.Courses;
using Adria.Infrastructure.WebApi.Controllers.Responses;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers;

public sealed class SearchCourseByIdController
{
    public static async Task<Results<Ok<CourseByIDResponse>, NotFound>> Invoke([FromRoute] Guid courseId,
        [FromServices] IUseCase<SearchCourseByIdInput, Task<CourseData>> searchCourseById)
    {
        CourseData courseData = await searchCourseById.Execute(new SearchCourseByIdInput(courseId));
        return TypedResults.Ok(ToResponse(courseData));
    }
    private static CourseByIDResponse ToResponse(CourseData courseData)
    {
        return new CourseByIDResponse(
            courseData.Id,
            courseData.Name,
            courseData.Level,
            courseData.Description,
            courseData.Category,
            courseData.Tags,
            courseData.PointsOnCompletion,
            courseData.LearnList,
            courseData.Duration,
            courseData.BannerImage,
            courseData.SubscriptionRequired
        );

    }
    private SearchCourseByIdController() { }
}