using Adria.Application.Contracts.Data;
using Adria.Application.Users;
using UnitTests.Mocks;

namespace UnitTests.Adria.Application.Users;

public class GetAllInterestsTest
{
    [Fact]
    public async Task ReturnsInterests()
    {
        var mock = new MockAllInterestsQuery()
        {
            Interests = new[] { new InterestData("gaming"), new InterestData("hunting"), new InterestData("mining") }
        };

        var mocklogger = new MockLogger<GetAllInterests>();
        var useCase = new GetAllInterests(mock, mocklogger);

        var result = await useCase.Execute();
        var list = result.ToList();

        Assert.Equal(3, list.Count);
        Assert.Contains(new InterestData("gaming"), list);
        Assert.Contains(new InterestData("hunting"), list);
        Assert.Contains(new InterestData("mining"), list);
    }
}