using Adria.Application.Contracts.Data;
using Adria.Application.Users;
using UnitTests.Mocks;

namespace UnitTests.Adria.Application.Users;

public class GetAllOccupationsTest
{
    [Fact]
    public async Task ReturnsOccupations()
    {
        var mock = new MockAllOccupationsQuery()
        {
            Occupations = new[] { new OccupationData("cop"), new OccupationData("chef"), new OccupationData("surgeon") }
        };

        var mocklogger = new MockLogger<GetAllOccupations>();
        var useCase = new GetAllOccupations(mock, mocklogger);

        var result = await useCase.Execute();
        var list = result.ToList();

        Assert.Equal(3, list.Count());
        Assert.Contains(new OccupationData("cop"), list);
        Assert.Contains(new OccupationData("chef"), list);
        Assert.Contains(new OccupationData("surgeon"), list);
    }
}