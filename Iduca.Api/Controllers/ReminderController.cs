using Iduca.Api.Enums;
using Iduca.Application.Features.Reminders.Create;
using Iduca.Application.Features.Reminders.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iduca.Api.Controllers;

[ApiController]
[Route(APIRoutes.Reminders)]
public class RemindersController(IMediator mediator) : ControllerBase
{
    private readonly IMediator mediator = mediator;

    [HttpPost]
    public async Task<ActionResult<CreateReminderResponse>> Create(
        CreateReminderRequest request, CancellationToken cancellationToken
    )
    {
        var response = await mediator.Send(request, cancellationToken);
        return Created(APIRoutes.Reminders, response);
    }

    [HttpPatch]
    [Route("{id}")]
    public async Task<ActionResult<UpdateReminderResponse>> Update (
        [FromRoute] Guid id, 
        [FromBody] UpdateReminderRequest request, 
        CancellationToken cancellationToken
    )
    {
        var response = await mediator.Send(new UpdateReminderRequest(id, request.NewTitle, request.NewDescription, request.NewDate), cancellationToken);
        return Ok(response);
    } 
}