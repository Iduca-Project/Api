using Iduca.Api.Enums;
using Iduca.Api.Attributes;
using Iduca.Application.Common.Services;
using Iduca.Application.Features.Companies.Create;
using Iduca.Application.Features.Courses.Create;
using Iduca.Application.Features.Courses.Delete;
using Iduca.Application.Features.Courses.Enroll;
using Iduca.Application.Features.Courses.Get;
using Iduca.Application.Features.Courses.GetByQuery;
using Iduca.Application.Features.Courses.GetDetails;
using Iduca.Application.Features.Courses.Update;
using Iduca.Domain.Common.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Iduca.Application.Features.Courses.GetCourseOfUser;

namespace Iduca.Api.Controllers;

[ApiController]
[Route(APIRoutes.Courses)]
[CustomAuthorize] // Requer autenticação para todas as rotas
public class CoursesController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IHierarchyService _hierarchyService;

    public CoursesController(IMediator mediator, IHierarchyService hierarchyService)
    {
        _mediator = mediator;
        _hierarchyService = hierarchyService;
    }

    /// <summary>
    /// Retorna a lista paginada de cursos com suporte a busca e filtros
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<GetCoursesResponse>> GetCourses(
        [FromQuery] int page = 1,
        [FromQuery] string? search = null,
        [FromQuery] Guid? category = null,
        [FromQuery] int? difficulty = null,
        [FromQuery] int maxItems = 10,
        CancellationToken cancellationToken = default
    )
    {
        
        var request = new GetCoursesRequest(
            search,
            difficulty,
            category,
            page,
            maxItems
        );
        
        var response = await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Retorna as informações gerais de um curso + lista de módulos
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<GetCourseDetailsResponse>> GetCourseById(
        [FromRoute] Guid id,
        [FromQuery] Guid? userId = null,
        CancellationToken cancellationToken = default
    )
    {
        if (userId is null)
            userId = GetCurrentUserId();
        var response = await _mediator.Send(new GetCourseDetailsRequest(id, userId), cancellationToken);
        return Ok(response);
    }


    [HttpGet("user")]
    public async Task<ActionResult<GetCourseDetailsResponse>> GetCourseById(
        CancellationToken cancellationToken
    )
    {
        var response = await _mediator.Send(new GetCourseOfUserCourseRequest(GetCurrentUserId()), cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Matricular usuário em um curso (self-enrollment ou por superior hierárquico)
    /// </summary>
    [HttpPost("{courseId}/enroll")]
    public async Task<ActionResult<EnrollCourseResponse>> Enroll(
        [FromRoute] Guid courseId,
        [FromBody] EnrollCourseRequestId request,
        CancellationToken cancellationToken
    )
    {
        // Verificar se o courseId da rota coincide com o do body
        if (courseId != request.CourseId)
            return BadRequest("CourseId na rota deve coincidir com o CourseId no body.");

        var currentUserId = GetCurrentUserId();
        
        // Verificar se o usuário atual pode matricular o usuário alvo
        // Pode matricular a si mesmo, ou se for superior hierárquico/admin
        if (request.UserId != currentUserId && !await _hierarchyService.CanUserAccessDataAsync(currentUserId, request.UserId, cancellationToken))
        {
            return Forbid("Você não tem permissão para matricular este usuário. Apenas administradores ou superiores hierárquicos podem matricular outros usuários.");
        }

        var response = await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }
}