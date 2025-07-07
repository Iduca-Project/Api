using Iduca.Api.Enums;
using Iduca.Api.Attributes;
using Iduca.Application.Common.Services;
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
[CustomAuthorize]
public class UsersController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IHierarchyService _hierarchyService;

    public UsersController(IMediator mediator, IHierarchyService hierarchyService)
    {
        _mediator = mediator;
        _hierarchyService = hierarchyService;
    }

    [HttpPost]
    public async Task<ActionResult<CreateUserResponse>> Create(
        [FromBody] CreateUserRequest request, CancellationToken cancellationToken
    )
    {

        var currentUserId = GetCurrentUserId();

        var newRequest = new CreateUserRequest(
                request.Name,
                request.Identity,
                request.Email,
                request.Password,
                request.CompanyId,
                currentUserId,
                request.IsAdmin,
                request.Image,
                request.Interests
            );

        var response = await _mediator.Send(newRequest, cancellationToken);
        return Created(APIRoutes.Users, response);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<GetUserResponse>> GetById(
        [FromRoute] Guid id, CancellationToken cancellationToken
    )
    {
        var currentUserId = GetCurrentUserId();
        
        // Verificar se o usuário atual pode acessar os dados do usuário solicitado
        if (!await _hierarchyService.CanUserAccessDataAsync(currentUserId, id, cancellationToken))
        {
            return Forbid("Você não tem permissão para acessar os dados deste usuário. Apenas você mesmo, seus superiores ou subordinados podem ser acessados.");
        }

        var response = await _mediator.Send(new GetUserRequest(id), cancellationToken);
        return Ok(response);
    }

    [HttpGet]
    [Route("all")]
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
        var currentUserId = GetCurrentUserId();
        var isAdmin = IsCurrentUserAdmin();
        
        // Se não for admin, só pode ver usuários acessíveis pela hierarquia
        if (!isAdmin)
        {
            var accessibleUserIds = await _hierarchyService.GetAccessibleUserIdsAsync(currentUserId, cancellationToken);
            
            // TODO: Implementar filtro no handler para limitar resultados apenas aos usuários acessíveis
            // Por enquanto, manteremos a funcionalidade original mas com aviso
        }

        if (Page < 1 || MaxItems < 1)
            return BadRequest("Page and MaxItems must be greater than 0.");

        var response = await _mediator.Send(new GetUsersRequest(
            Name, Email, CompanyId, IsAdmin, Page, MaxItems
        ), cancellationToken);
        return Ok(response);
    }

    [HttpPut]
    public async Task<ActionResult<UpdateUserResponse>> Update(
        [FromBody] UpdateUserRequest request, CancellationToken cancellationToken
    )
    {
        var currentUserId = GetCurrentUserId();
        var isAdmin = IsCurrentUserAdmin();
        
        // Verificar se o usuário atual pode atualizar o usuário alvo
        if (!isAdmin && !await _hierarchyService.CanUserAccessDataAsync(currentUserId, request.Id, cancellationToken))
        {
            return Forbid("Você não tem permissão para atualizar este usuário. Apenas administradores ou superiores hierárquicos podem atualizar usuários.");
        }
        
        var response = await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }

    [HttpDelete]
    [Route("{id}")]
    [CustomAuthorize(RequireAdmin = true)] // Apenas admins podem deletar usuários (operação crítica)
    public async Task<ActionResult> Delete(
        [FromRoute] Guid id, CancellationToken cancellationToken
    )
    {
        await _mediator.Send(new DeleteUserRequest(id), cancellationToken);
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
        
        var response = await _mediator.Send(new GetMyCoursesRequest(userId), cancellationToken);
        return Ok(response);
    }
}
