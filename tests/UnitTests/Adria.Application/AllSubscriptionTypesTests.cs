using Adria.Application.Contracts.Data;
using Adria.Application.Subscriptions;
using UnitTests.Mocks;


namespace UnitTests;

public class AllSubscriptionTypesTests
{
    [Fact]
    public async Task ReturnSubscriptions()
    {
        SubscriptionData subscription = new("Base", 2);
        SubscriptionData subscription2 = new("Premium", 2);

        IList<SubscriptionData> subscriptions = new List<SubscriptionData> { subscription, subscription2 };

        var mockquery = new MockAllSubscriptionQuerie();
        mockquery.SetSubscriptions(subscriptions);
        var mocklogger = new MockLogger<AllSubscriptionTypes>();
        var usecase = new AllSubscriptionTypes(mockquery, mocklogger);

        var result = await usecase.Execute();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Contains(result, sub => sub.Type == "Base");
        Assert.Contains(result, sub => sub.Type == "Premium");
        Assert.Single(mocklogger.LoggedMessages);
    }


    [Fact]
    public async Task NoSubscriptions()
    {
        IList<SubscriptionData> subscriptions = new List<SubscriptionData>();

        var mockquery = new MockAllSubscriptionQuerie();
        mockquery.SetSubscriptions(subscriptions);
        var mocklogger = new MockLogger<AllSubscriptionTypes>();
        var usecase = new AllSubscriptionTypes(mockquery, mocklogger);

        var result = await usecase.Execute();

        Assert.NotNull(result);
        Assert.Empty(result);
        Assert.Single(mocklogger.LoggedMessages);
    }

}
