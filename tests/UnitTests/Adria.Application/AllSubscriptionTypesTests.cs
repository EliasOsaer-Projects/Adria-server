
using Adria.Application.Contracts.Data;
using Adria.Application.Subscriptions;

namespace UnitTests;

public class AllSubscriptionTypesTests
{
    [Fact]
    public async Task ReturnSubscriptions()
    {
        SubscriptionData subscription = new("Base", 2, "base sub");
        SubscriptionData subscription2 = new("Base", 2, "base sub");

        IList<SubscriptionData> subscriptions = new List<SubscriptionData> { subscription, subscription2 };



    }

}
