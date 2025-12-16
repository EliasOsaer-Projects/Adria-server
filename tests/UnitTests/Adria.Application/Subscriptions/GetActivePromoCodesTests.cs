using Adria.Application.Subscriptions;
using UnitTests.Mocks;

namespace UnitTests.Adria.Application.Subscriptions;

public class GetActivePromoCodesTests
{
    [Fact]
    public async Task Should_ReturnActivePromoCodes()
    {
        var mock = new MockCurrentPromoCodesQuery
        {
            PromoCodes = new[] { "TEST1", "TEST2" }
        };

        var mocklogger = new MockLogger<GetActivePromoCodes>();
        var useCase = new GetActivePromoCodes(mock, mocklogger);

        var result = await useCase.Execute();
        var list = result.ToList();

        Assert.Equal(2, list.Count);
        Assert.Contains("TEST1", list);
        Assert.Contains("TEST2", list);
    }
}