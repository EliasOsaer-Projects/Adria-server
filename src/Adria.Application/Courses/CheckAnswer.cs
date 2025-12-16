using Adria.Application.Contracts;
using Adria.Domain;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Courses;

public sealed record CheckAnswerInput(Guid QuestionId, Guid AnswerId, Guid UserId, Guid CourseId);

public class CheckAnswer
    : IUseCase<CheckAnswerInput, Task<Guid>>
{
    private readonly ILogger<CheckAnswer> _logger;
    private readonly IUserQARepository _userQARepository;
    private readonly IUserCourseRepository _userCourseRepository;

    public CheckAnswer(
        ILogger<CheckAnswer> logger,
        IUserQARepository userQaRepository,
        IUserCourseRepository userCourseRepository
    )
    {
        _logger = logger;
        _userQARepository = userQaRepository;
        _userCourseRepository = userCourseRepository;
    }
    
    public async Task<Guid> Execute(CheckAnswerInput input)
    {
        var correctAnswerId = await _userQARepository.Save(
                                                        input.UserId,
                                                        input.AnswerId,
                                                        input.QuestionId
                                                    );
        var progress = await _userQARepository.GetProgress(input.UserId, input.CourseId);
        var userCourse = await _userCourseRepository.ById(input.UserId, input.CourseId);
        
        await _userCourseRepository.Save(userCourse, progress);
        
        return correctAnswerId;
    }
}