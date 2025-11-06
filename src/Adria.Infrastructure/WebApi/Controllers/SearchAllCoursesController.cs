using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers;

public sealed class SearchAllCoursesController
{
    public static async Task<Results<Ok<List<Course>>, NotFound>> Invoke(
        [FromServices] IUseCase<Task<IReadOnlyCollection<CourseData>>> searchAllCourses
    )
    {
        IReadOnlyCollection<CourseData> CourseData = await searchAllCourses.Execute();

        if (CourseData == null || CourseData.Count == 0)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(ToResponse(CourseData));
    }

    private static List<Course> ToResponse(IReadOnlyCollection<CourseData> CourseData)
    {
        return CourseData.Select(data => new Course(
            id: data.Id,
            name: data.Name,
            level: data.Level,
            category: data.Category,
            description: data.Description,
            pointsOnCompletion: data.PointsOnCompletion,
            duration: data.Duration,
            learnList: data.LearnList,
            modules: data.Modules
        )).ToList();
    }
    
    
    private SearchAllCoursesController()
    {
    }
}