using Adria.Application.Contracts.Data;

namespace Adria.Application.Contracts;



public interface IAllCoursesQuery
{
    Task<IReadOnlyCollection<CourseData>> Fetch();
}