using Adria.Domain;
using Adria.Domain.Users;
using Adria.Domain.Shared.Exceptions;
using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Microsoft.Extensions.Logging;

namespace Adria.Application;

public sealed record AddProgressToCourseInput(Guid userId, Guid courseId);

public sealed class AddProgressToCourse : IUseCase<AddProgressToCourseInput, Task<Guid>>
{
    IUserCourseRepository _userCourseRepository;
    IUserRepository _userRepository;
    ILogger _logger;

    public AddProgressToCourse(IUserRepository user, IUserCourseRepository userCourse, ILogger logger)
    {
        _userRepository = user;
        _userCourseRepository = userCourse;
        _logger = logger;
    }

    public async Task<Guid> Execute(AddProgressToCourseInput input)
    {
        User? user = (await _userRepository.ById(input.userId)) ?? throw new ElementNotFoundException($"user with id {input.userId} not found.");

        UserCourse? userCourse = (await _userCourseRepository.ById(input.courseId)) ?? throw new ElementNotFoundException($"userCourse with id {input.courseId} not found");

        _logger.LogInformation("vhanging data");
        await _userCourseRepository.Save(user, userCourse);
        return userCourse.AdriaId;
    }
}
