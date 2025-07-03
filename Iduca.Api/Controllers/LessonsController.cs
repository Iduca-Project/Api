using Iduca.Api.Enums;
using Iduca.Api.Attributes;
using Iduca.Application.Features.Lessons.Complete;
using Iduca.Application.Features.Lessons.Create;
using Iduca.Application.Features.Lessons.DeleteById;
using Iduca.Application.Features.Lessons.GetByModuleId;
using Iduca.Application.Features.Lessons.GetByModuleByUser;
using Iduca.Application.Features.Lessons.GetDetails;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iduca.Api.Controllers;

[ApiController]
[Route("api")]
[CustomAuthorize] // Requer autenticação para todas as rotas
public class LessonsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator mediator = mediator;

    /// <summary>
    /// Retorna os dados completos de uma aula + info sobre a próxima aula
    /// </summary>
    [HttpGet("lessons/{id}")]
    public async Task<ActionResult<GetLessonDetailsResponse>> GetLessonById(
        [FromRoute] Guid id,
        [FromQuery] Guid? userId = null,
        CancellationToken cancellationToken = default
    )
    {
        var response = await mediator.Send(new GetLessonDetailsRequest(id, userId), cancellationToken);
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<CreateLessonResponse>> Create(
        CreateLessonRequest request, CancellationToken cancellationToken
    )
    {
        var response = await mediator.Send(request, cancellationToken);
        return Ok(response);
    }

    [HttpGet("module/{Id}")]
    public async Task<ActionResult<GetByModuleIdLessonResponse>> GetByModule(
        [FromRoute] Guid Id, CancellationToken cancellationToken
    )
    {
        var response = await mediator.Send(new GetByModuleIdLessonRequest(Id), cancellationToken);
        return Ok(response);
    }

    [HttpGet("{Id}")]
    public async Task<ActionResult<GetByModuleByUserLessonResponse>> GetByModuleByUser(
        [FromRoute] Guid Id, CancellationToken cancellationToken
    )
    {
        var response = await mediator.Send(new GetByModuleByUserLessonRequest(Id, new Guid("5374148b-5061-11f0-b52d-0a002700000b")), cancellationToken);
        return Ok(response);
    }

    [HttpDelete("{Id}")]
    public async Task<ActionResult<DeleteByIdLessonResponse>> DeleteById(
        [FromRoute] Guid Id, CancellationToken cancellationToken
    )
    {
        await mediator.Send(new DeleteByIdLessonRequest(Id), cancellationToken);
        return NoContent();
    }

    [HttpPost]
    [Route("{lessonId}/complete")]
    public async Task<ActionResult<CompleteLessonResponse>> CompleteLesson(
        [FromRoute] Guid lessonId,
        [FromBody] CompleteLessonRequest request,
        CancellationToken cancellationToken
    )
    {
        // Verificar se o lessonId da rota coincide com o do body
        if (lessonId != request.LessonId)
            return BadRequest("LessonId na rota deve coincidir com o LessonId no body.");

        var response = await mediator.Send(request, cancellationToken);
        return Ok(response);
    }
}