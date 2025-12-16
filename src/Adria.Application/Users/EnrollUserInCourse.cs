using Adria.Application.Contracts;
using Adria.Domain;
using Adria.Domain.Contracts;
using Adria.Domain.PushNotifications;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Users;

public sealed record EnrollUserInCourseInput(Guid UserId, Guid CourseId);

public class EnrollUserInCourse 
    : IUseCase<EnrollUserInCourseInput, Task<UserCourseRepoData>>
{
   private readonly ILogger<EnrollUserInCourse> _logger;
    private readonly IUserCourseRepository _userCourseRepository;
    private readonly IPushSubscriptionRepository _pushSubscriptionRepository;
    private readonly IPushNotificationSender _pushNotificationSender;
    
    public EnrollUserInCourse(
        ILogger<EnrollUserInCourse> logger,
        IUserCourseRepository userCourseRepository,
        IPushSubscriptionRepository pushSubscriptionRepository,
        IPushNotificationSender pushNotificationSender
    )
    {
        _logger = logger;
        _userCourseRepository = userCourseRepository;
        _pushSubscriptionRepository = pushSubscriptionRepository;
        _pushNotificationSender = pushNotificationSender;
    }

    public async Task<UserCourseRepoData> Execute(EnrollUserInCourseInput input)
    {
        _logger.LogInformation("Enrolling user {UserId} in course {CourseId}", input.UserId, input.CourseId);
        
        UserCourseRepoData repoData = await _userCourseRepository.Save(input.UserId, input.CourseId);

        IReadOnlyList<PushSubscription> subscriptions;
        try
        {
            subscriptions = _pushSubscriptionRepository.GetSubscriptions();
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "Failed to read push subscriptions while enrolling user {UserId} in course {CourseId}",
                input.UserId,
                input.CourseId
            );
            return repoData;
        }

        if (subscriptions.Count == 0)
            return repoData;

        string title = "New course enrollment";
        string message = $"A learner just enrolled into course {input.CourseId}.";
        string url = $"/courses/{input.CourseId}";

        foreach (PushSubscription subscription in subscriptions)
        {
            try
            {
                _pushNotificationSender.SendNotification(subscription, title, message, url);
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    exception,
                    "Failed to send push notification for subscription {Endpoint}",
                    subscription.Endpoint
                );
            }
        }

        return repoData;
    }
}
