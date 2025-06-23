using Iduca.Api.Enums;
using Iduca.Application.Features.Lessons.Create;
using Iduca.Application.Features.Lessons.DeleteById;
using Iduca.Application.Features.Lessons.GetByModuleId;
using Iduca.Application.Features.Lessons.GetByModuleByUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iduca.Api.Controllers;

[ApiController]
[Route(APIRoutes.Lessons)]
public class LesosnsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator mediator = mediator;

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
}