using Adria.Application;
using Adria.Application.Courses;
using Adria.Application.Contracts.Data;
using UnitTests.Mocks;

namespace UnitTests;

public class GetlessonCountTest
{
    [Fact]
    public async Task Execute_Query()
    {

        var _mocklogger = new MockLogger<GetLessonsCount>();
        var _mockgetAllLessons = new MockGetLessonCountQuery();

        _mockgetAllLessons.getAmount(10);

        var Usecase = new GetLessonsCount(_mockgetAllLessons, _mocklogger);

        var result = await Usecase.Execute(new GetLessonsCountInput(Guid.NewGuid()));

        Assert.NotEqual(0, result);
        Assert.IsType<int>(result);
        Assert.IsNotType<DateTime>(result);
    }

}
