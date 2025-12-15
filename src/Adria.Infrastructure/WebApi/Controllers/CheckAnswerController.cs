using Adria.Application.Contracts;
using Adria.Application.Courses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Adria.Infrastructure.WebApi.Controllers;

public class CheckAnswerController
{
    private CheckAnswerController(){}
    
    public static async Task<Results<Ok<Guid>, NotFound>> Invoke(
        [FromRoute] Guid questionId,
        [FromRoute] Guid answerId,
        [FromRoute] Guid userId,
        [FromServices] IUseCase<CheckAnswerInput, Task<Guid>> checkAnswer
    )
    {
        Guid correctAnswer =  await checkAnswer.Execute(new CheckAnswerInput(questionId, answerId, userId));
        return TypedResults.Ok(correctAnswer);
    }
}