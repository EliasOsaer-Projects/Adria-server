namespace Adria.Domain;

public interface IUserCourseRepository
{
    Task<UserCourse?> ById(Guid userCourseId);
    Task Save(User user, UserCourse userCourse);
}
