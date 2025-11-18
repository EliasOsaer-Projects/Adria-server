using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers;

public sealed class SearchAllCoursesController
{
    public static async Task<Results<Ok<List<CourseData>>, NotFound>> Invoke(
        [FromServices] IUseCase<Task<IReadOnlyCollection<CourseData>>> searchAllCourses,
        [FromQuery] string? category = null,
        [FromQuery] string? tag = null
    )
    {
        IReadOnlyCollection<CourseData> CourseData = await searchAllCourses.Execute();

        if (CourseData == null || CourseData.Count == 0)
        {
            return TypedResults.NotFound();
        }

        IEnumerable<CourseData> filteredData = CourseData;

       if (!string.IsNullOrWhiteSpace(category))
       {
           filteredData = filteredData.Where(c=> c.Category == category);
       }
       
       
       if (!string.IsNullOrWhiteSpace(tag))
       {
           filteredData = filteredData.Where(t=> t.Tags.Contains(tag));
       }
    
        return TypedResults.Ok(filteredData.ToList());
    }
}