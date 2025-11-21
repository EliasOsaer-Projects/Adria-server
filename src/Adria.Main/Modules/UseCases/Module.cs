using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Application.Courses;
using Adria.Application.Users;
using Adria.Application.Subscriptions;

namespace Adria.Main.Modules.UseCases;

public static class UseCases
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        return services
            .AddScoped<IUseCase<SearchSubscriptionTypeByIdInput, Task<SubscriptionData>>, SearchSubscriptionTypeById>()
            .AddScoped<IUseCase<Task<IReadOnlyCollection<SubscriptionData>>>, AllSubscriptionTypes>()
            .AddScoped<IUseCase<EnrollUserInCourseInput, Task<UserCourseData>>, EnrollUserInCourse>()
            
            .AddScoped<IUseCase<SearchUserByIdInput, Task<UserData>>, SearchUserById>()
            .AddScoped<IUseCase<Task<IEnumerable<LeaderBoardData>>>, Top10Users>()
            
            .AddScoped<IUseCase<Task<IReadOnlyCollection<CourseData>>>, SearchAllCourses>()
            .AddScoped<IUseCase<SearchUserCoursesByUserInput, Task<IReadOnlyCollection<UserCourseData>>>, SearchUserCoursesByUser>()
            .AddScoped<IUseCase<SearchCourseByIdInput, Task<CourseData>>, SearchCourseById>()
            .AddScoped<IUseCase<GetCourseModulesInput, Task<IEnumerable<CourseModuleData>>>, GetCourseModules>();
    }
}