using Iduca.Api.Enums;
using Iduca.Application.Features.User.Create;
using Iduca.Application.Features.User.Delete;
using Iduca.Application.Features.User.Get;
using Iduca.Application.Features.User.GetByQuery;
using Iduca.Application.Features.User.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iduca.Api.Controllers;

[ApiController]
[Route(APIRoutes.Users)]
public class UsersController(IMediator mediator) : ControllerBase
{
    private readonly IMediator mediator = mediator;

    [HttpPost]
    public async Task<ActionResult<CreateUserResponse>> Create(
        CreateUserRequest request, CancellationToken cancellationToken
    )
    {
        var response = await mediator.Send(request, cancellationToken);
        return Created(APIRoutes.Users, response);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<GetUserResponse>> GetById(
        [FromRoute] Guid id, CancellationToken cancellationToken
    )
    {
        var response = await mediator.Send(new GetUserRequest(id), cancellationToken);
        return Ok(response);
    }

    [HttpGet]
    [Route("all")]
    public async Task<ActionResult<List<GetUsersResponse>>> GetAll(
        [FromQuery] string? Name,
        [FromQuery] string? Email,
        [FromQuery] Guid? CompanyId,
        [FromQuery] bool? IsAdmin,
        [FromQuery] int Page = 1,
        [FromQuery] int MaxItems = 10,
        CancellationToken cancellationToken = default
    )
    {
        if (Page < 1 || MaxItems < 1)
            return BadRequest("Page and MaxItems must be greater than 0.");

        var response = await mediator.Send(new GetUsersRequest(Name, Email, CompanyId, IsAdmin, Page, MaxItems), cancellationToken);
        return Ok(response);
    }

    [HttpPut]
    public async Task<ActionResult<UpdateUserResponse>> Update(
        [FromBody] UpdateUserRequest request, CancellationToken cancellationToken
    )
    {
        var response = await mediator.Send(request, cancellationToken);
        return Ok(response);
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<ActionResult> Delete(
        [FromRoute] Guid id, CancellationToken cancellationToken
    )
    {
        await mediator.Send(new DeleteUserRequest(id), cancellationToken);
        return NoContent();
    }
}
