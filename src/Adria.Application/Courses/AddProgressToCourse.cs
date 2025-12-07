using Adria.Domain;
using Adria.Domain.Users;
using Adria.Domain.Shared.Exceptions;
using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Microsoft.Extensions.Logging;

namespace Adria.Application;

public sealed record AddProgressToCourseInput(Guid userId, Guid courseId, int progress);

public sealed class AddProgressToCourse : IUseCase<AddProgressToCourseInput, Task<Guid>>
{
    IUserCourseRepository _userCourseRepository;
    ILogger<AddProgressToCourse> _logger;

    public AddProgressToCourse(IUserCourseRepository userCourse, ILogger<AddProgressToCourse> logger)
    {
        _userCourseRepository = userCourse;
        _logger = logger;
    }

    public async Task<Guid> Execute(AddProgressToCourseInput input)
    {
        UserCourse? userCourse = (await _userCourseRepository.ById(input.userId, input.courseId)) ?? throw new ElementNotFoundException($"userCourse with id {input.courseId} not found");

        _logger.LogInformation("vhanging data");
        await _userCourseRepository.Save(userCourse, input.progress);
        return userCourse.AdriaId;
    }
}
