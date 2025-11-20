using Adria.Domain;

namespace UnitTests.Adria.Domain;

public class UserSubscriptionTest
{
    private readonly Subscription _subscription;
    private readonly DateTime _startDate;
    private readonly DateTime _endDate;
    private readonly bool _status;

    public UserSubscriptionTest()
    {
        _subscription = new Subscription(
            Guid.NewGuid(),
            "Premium",
            49.99m,
            "Premium subscription for 1 month.",
            true
        );
        _startDate = new DateTime(2024, 1, 1);
        _endDate =  _startDate.AddDays(30);
        _status = true;
    }

    [Fact]
    public void Constructor_WithValidParameters_SetsAllProperties()
    {
        // Act
        var userSubscription = new UserSubscription(
            _subscription,
            _startDate
        );

        // Assert
        Assert.Equal(_subscription, userSubscription.CurrentSubscription);
        Assert.Equal(_startDate, userSubscription.StartDate);
        Assert.Equal(_endDate, userSubscription.EndDate);
        Assert.Equal(_status, userSubscription.Status);
    }

}