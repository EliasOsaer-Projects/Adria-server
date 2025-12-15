namespace Adria.Application.Contracts.Data;

public sealed record QuestionAnswerData(
    Guid AnswerId,
    string Answer
);