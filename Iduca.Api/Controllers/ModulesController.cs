using Iduca.Api.Enums;
using Iduca.Application.Features.Modules.DeleteById;
using Iduca.Application.Features.Modules.Create;
using Iduca.Application.Features.Modules.GetById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iduca.Api.Controllers;

[ApiController]
[Route(APIRoutes.Modules)]
public class ModulesController(IMediator mediator) : ControllerBase
{
    private readonly IMediator mediator = mediator;

    [HttpPost]
    public async Task<ActionResult<CreateModuleResponse>> Create(
        CreateModuleRequest request, CancellationToken cancellationToken
    )
    {
        var response = await mediator.Send(request, cancellationToken);
        return Ok(response);
    }
    
    [HttpGet]
    [Route("course/{Id}")]
    public async Task<ActionResult<GetByCourseIdModuleResponse>> GetByCourseId(
        [FromRoute] Guid Id, CancellationToken cancellationToken
    )
    {
        var response = await mediator.Send(new GetByCourseIdModuleRequest(Id), cancellationToken);
        return Ok(response);
    }
    [HttpGet]
    [Route("{Id}")]
    public async Task<ActionResult<GetByIdModuleResponse>> GetById(
        [FromRoute] Guid Id, CancellationToken cancellationToken
    )
    {
        var response = await mediator.Send(new GetByIdModuleRequest(Id), cancellationToken);
        return Ok(response);
    }

    [HttpDelete]
    [Route("{Id}")]
    public async Task<ActionResult<DeleteByIdModuleResponse>> DeleteById(
        [FromRoute] Guid Id, CancellationToken cancellationToken
    )
    {
        await mediator.Send(new DeleteByIdModuleRequest(Id), cancellationToken);
        return NoContent();
    }
}