using Adria.Domain;
using Adria.Domain.Users;
using Adria.Application.Contracts;
using Adria.Application.Contracts.Data;
using Microsoft.Extensions.Logging;

namespace Adria.Application;

public sealed record AddProgressToCourseInput(Guid id);

public class AddProgressToCourse : IUseCase<AddProgressToCourseInput, Task<Guid>>
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
    //public async Task<Guid> Execute(AddProgressToCourseInput input){
    //}
}
