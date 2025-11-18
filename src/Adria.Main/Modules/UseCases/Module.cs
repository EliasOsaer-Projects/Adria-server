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
            .AddScoped<IUseCase<SearchSubscriptionTypeByIdInput, Task<IReadOnlyCollection<SubscriptionData>>>,
                SearchSubscriptionTypeById>()
            .AddScoped<IUseCase<SearchUserByIdInput, Task<UserData>>, SearchUserById>()
            .AddScoped<IUseCase<Task<IReadOnlyCollection<SubscriptionData>>>, AllSubscriptionTypes>()
            .AddScoped<IUseCase<Task<IEnumerable<LeaderBoardData>>>, Top10Users>()
            .AddScoped<IUseCase<Task<IReadOnlyCollection<CourseData>>>, SearchAllCourses>()
            .AddScoped<IUseCase<SearchUserCoursesByUserInput, Task<IReadOnlyCollection<UserData>>>,
                SearchCustomerCoursesByUser>();
    }
}