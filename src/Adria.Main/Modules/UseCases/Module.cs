using Adria.Application.Subscriptions;
using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Application.Courses;

namespace Adria.Main.Modules.UseCases;

public static class UseCases
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        return services
            .AddScoped<IUseCase<Task<IReadOnlyCollection<SubscriptionData>>>, AllSubscriptionTypes>()
            .AddScoped<IUseCase<Task<IReadOnlyCollection<CourseData>>>, SearchAllCourses>();
    }
}
