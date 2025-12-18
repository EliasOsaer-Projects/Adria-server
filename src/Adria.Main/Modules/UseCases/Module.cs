using Adria.Application;
using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Adria.Application.Courses;
using Adria.Application.Users;
using Adria.Application.Subscriptions;
using Mysqlx.Crud;
using Adria.Application.PushNotifications;

namespace Adria.Main.Modules.UseCases;

public static class UseCases
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        return services
            // Subscription Use Cases
            .AddScoped<IUseCase<SearchSubscriptionTypeByIdInput, Task<SubscriptionData>>, SearchSubscriptionTypeById>()
            .AddScoped<IUseCase<Task<IReadOnlyCollection<SubscriptionData>>>, AllSubscriptionTypes>()
            .AddScoped<IUseCase<SubscribeUserInput, Task>, SubscribeUser>()
            .AddScoped<IUseCase<Task<IEnumerable<string>>>, GetActivePromoCodes>()
            .AddScoped<IUseCase<GetDiscountByPromoCodeInput, Task<decimal>>, GetDiscountByPromoCode>()

            // Course Use Cases
            .AddScoped<IUseCase<EnrollUserInCourseInput, Task<Domain.Contracts.UserCourseRepoData>>, EnrollUserInCourse>()
            .AddScoped<IUseCase<AddProgressToCourseInput, Task>, AddProgressToCourse>()
            .AddScoped<IUseCase<SearchAllCoursesInput, Task<IReadOnlyCollection<CourseData>>>, SearchAllCourses>()
            .AddScoped<IUseCase<SearchUserCoursesByUserInput, Task<IReadOnlyCollection<UserCourseData>>>, SearchUserCoursesByUser>()
            .AddScoped<IUseCase<SearchCourseByIdInput, Task<CourseData>>, SearchCourseById>()
            .AddScoped<IUseCase<GetCourseModulesInput, Task<IEnumerable<CourseModuleData>>>, GetCourseModules>()
            .AddScoped<IUseCase<GetNextCourseModuleInput, Task<CourseModuleData>>, GetNextCourseModule>()
            .AddScoped<IUseCase<CheckAnswerInput, Task<Guid>>, CheckAnswer>()
            .AddScoped<IUseCase<GetEnrollmentCountInput, Task<int>>, GetEnrollmentCount>()
            .AddScoped<IUseCase<GetLessonsCountInput, Task<int>>, GetLessonsCount>()
            
            // User Use Cases
            .AddScoped<IUseCase<UpdateUserProfilePictureInput, Task>, UpdateUserProfilePicture>()
            .AddScoped<IUseCase<Guid, Task>, DeleteUserProfilePicture>()
            .AddScoped<IUseCase<SearchUserByIdInput, Task<UserData>>, SearchUserById>()
            .AddScoped<IUseCase<Task<IEnumerable<LeaderBoardData>>>, Top10Users>()
            .AddScoped<IUseCase<AddOccupationInput, Task>, AddOccupation>()
            .AddScoped<IUseCase<AddInterestsInput, Task>, AddInterests>()
            .AddScoped<IUseCase<Task<IReadOnlyCollection<OccupationData>>>, GetAllOccupations>()
            .AddScoped<IUseCase<Task<IReadOnlyCollection<InterestData>>>, GetAllInterests>()
            .AddScoped<IUseCase<Guid, Task<bool>>, IsActiveUser>()
            .AddScoped<IUseCase<CancelUserSubscriptionInput, Task>, CancelUserSubscription>()

            // Cosmetic Use Cases
            .AddScoped<IUseCase<AllCosmeticsInput, Task<IReadOnlyCollection<CosmeticItemData>>>, AllCosmetics>()
            .AddScoped<IUseCase<PurchaseCosmeticInput, Task>, PurchaseCosmetic>()
            .AddScoped<IUseCase<GetUserCosmeticsInput, Task<IReadOnlyCollection<CosmeticItemData>>>, GetUserCosmetics>();

    }
}
