using Iduca.Api.Enums;
using Iduca.Application.Features.Reminders.Create;
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

    // Additional methods for getting, updating, and deleting reminders can be added here.
}