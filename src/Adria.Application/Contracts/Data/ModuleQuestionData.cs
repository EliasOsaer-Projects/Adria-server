namespace Adria.Application.Contracts.Data;

public sealed record ModuleQuestionData(
    Guid QuestionId,
    string Question,
    IReadOnlyCollection<QuestionAnswerData> Answers
);