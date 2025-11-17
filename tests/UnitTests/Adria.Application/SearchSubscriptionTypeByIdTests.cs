using Adria.Application.Contracts.Data;
using Adria.Application.Subscriptions;
using UnitTests.Mocks;

namespace UnitTests;

public class SearchSubscriptionTypeByIdTests
{

    [Fact]
    public async Task GetSubscription()
    {
        Guid _id = Guid.NewGuid();
        Guid _id2 = Guid.NewGuid();
        Guid _id3 = Guid.NewGuid();
        SubscriptionData subscription = new(_id, "Base", 2, "base sub", false);
        SubscriptionData subscription2 = new(_id2, "Premium", 10, "premium sub", true);
        SubscriptionData subscription3 = new(_id3, "Super", 9001, "SUPER! sub!", false);

        IDictionary<Guid, SubscriptionData> subscriptions = new Dictionary<Guid, SubscriptionData>();

        subscriptions.Add(_id, subscription);
        subscriptions.Add(_id2, subscription2);
        subscriptions.Add(_id3, subscription3);

        MockSearchSubscriptionByIdQuery searchSubscriptionByIdQuery = new();
        searchSubscriptionByIdQuery.SetSubscriptions(subscriptions);
        var mocklogger = new MockLogger<SearchSubscriptionTypeById>();

        var usecase = new SearchSubscriptionTypeById(searchSubscriptionByIdQuery, mocklogger);

        var result = await usecase.Execute(new(_id));

        Assert.Contains(result, sub => sub.Type == "Base");
    }

}
