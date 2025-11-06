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
            .AddScoped<IUseCase<Task<IReadOnlyCollection<SubscriptionData>>>, AllSubscriptionTypes>()
            .AddScoped<IUseCase<Task<IReadOnlyCollection<CourseData>>>, SearchAllCourses>()
            .AddScoped<IUseCase<SearchUserByIdInput, Task<UserData>>, SearchUserById>();
    }
}
