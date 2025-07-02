using Iduca.Api.Enums;
using Iduca.Api.Attributes;
using Iduca.Application.Features.User.Create;
using Iduca.Application.Features.User.Delete;
using Iduca.Application.Features.User.Get;
using Iduca.Application.Features.User.GetByQuery;
using Iduca.Application.Features.User.Update;
using Iduca.Application.Features.Users.GetMyCourses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iduca.Api.Controllers;

[ApiController]
[Route(APIRoutes.Users)]
[CustomAuthorize] // Todas as rotas de usuário precisam de autenticação
public class UsersController(IMediator mediator) : ControllerBase
{
    private readonly IMediator mediator = mediator;

    [HttpPost]
    [CustomAuthorize(RequireAdmin = true)] // Apenas admins podem criar usuários
    public async Task<ActionResult<CreateUserResponse>> Create(
        [FromBody] CreateUserRequest request, CancellationToken cancellationToken
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
    [CustomAuthorize(RequireAdmin = true)] // Apenas admins podem listar todos os usuários
    public async Task<ActionResult<GetUsersResponse>> GetAll(
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

        var response = await mediator.Send(new GetUsersRequest(
            Name, Email, CompanyId, IsAdmin, Page, MaxItems
        ), cancellationToken);
        return Ok(response);
    }

    [HttpPut]
    [CustomAuthorize(RequireAdmin = true)] // Apenas admins podem atualizar usuários
    public async Task<ActionResult<UpdateUserResponse>> Update(
        [FromBody] UpdateUserRequest request, CancellationToken cancellationToken
    )
    {
        var response = await mediator.Send(request, cancellationToken);
        return Ok(response);
    }

    [HttpDelete]
    [Route("{id}")]
    [CustomAuthorize(RequireAdmin = true)] // Apenas admins podem deletar usuários
    public async Task<ActionResult> Delete(
        [FromRoute] Guid id, CancellationToken cancellationToken
    )
    {
        await mediator.Send(new DeleteUserRequest(id), cancellationToken);
        return NoContent();
    }

    [HttpGet]
    [Route("my-courses")]
    public async Task<ActionResult<GetMyCoursesResponse>> GetMyCourses(
        CancellationToken cancellationToken
    )
    {
        // TODO: Pegar o userId da sessão/token JWT
        var userId = new Guid("5374148b-5061-11f0-b52d-0a002700000b"); // TEMPORÁRIO - ID do admin
        
        var response = await mediator.Send(new GetMyCoursesRequest(userId), cancellationToken);
        return Ok(response);
    }
}
