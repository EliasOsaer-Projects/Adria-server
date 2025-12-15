using Adria.Application.Contracts;
using Adria.Domain;
using Microsoft.Extensions.Logging;

namespace Adria.Application.Courses;

public sealed record CheckAnswerInput(Guid QuestionId, Guid AnswerId, Guid UserId);

public class CheckAnswer
    : IUseCase<CheckAnswerInput, Task<Guid>>
{
    private readonly ILogger<CheckAnswer> _logger;
    private readonly IUserQARepository _userQARepository;

    public CheckAnswer(
        ILogger<CheckAnswer> logger,
        IUserQARepository userQaRepository
    )
    {
        _logger = logger;
        _userQARepository = userQaRepository;
    }
    
    public async Task<Guid> Execute(CheckAnswerInput input)
    {
        return await _userQARepository.Save(
            input.UserId,
            input.AnswerId,
            input.QuestionId
        );
    }
}