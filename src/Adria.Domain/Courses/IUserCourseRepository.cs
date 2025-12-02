namespace Adria.Domain;

public interface IUserCourseRepository
{
    Task Save(UserCourse userCourse, int progress);
}
