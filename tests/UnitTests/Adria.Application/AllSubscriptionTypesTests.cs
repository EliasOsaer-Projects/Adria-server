using Adria.Application.Contracts.Data;
using Adria.Application.Subscriptions;
using UnitTests.Mocks;


namespace UnitTests;

public class AllSubscriptionTypesTests
{
    [Fact]
    public async Task ReturnSubscriptions()
    {
        SubscriptionData subscription = new(Guid.NewGuid(), "Base", 2, "base sub", false);
        SubscriptionData subscription2 = new(Guid.NewGuid(), "Premium", 2, "base sub", true);

        IList<SubscriptionData> subscriptions = new List<SubscriptionData> { subscription, subscription2 };

        var mockquery = new MockAllSubscriptionQuery();
        mockquery.SetSubscriptions(subscriptions);
        var mocklogger = new MockLogger<AllSubscriptionTypes>();
        var usecase = new AllSubscriptionTypes(mockquery, mocklogger);

        var result = await usecase.Execute();

        Assert.NotNull(result);

    }


    [Fact]
    public async Task NoSubscriptions()
    {
        IList<SubscriptionData> subscriptions = new List<SubscriptionData>();

        var mockquery = new MockAllSubscriptionQuery();
        mockquery.SetSubscriptions(subscriptions);
        var mocklogger = new MockLogger<AllSubscriptionTypes>();
        var usecase = new AllSubscriptionTypes(mockquery, mocklogger);

        var result = await usecase.Execute();

        Assert.NotNull(result);
        Assert.Empty(result);
        Assert.Single(mocklogger.LoggedMessages);
    }

}
