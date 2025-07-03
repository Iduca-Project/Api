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
        [FromQuery] string? category = null,
        [FromQuery] int? difficulty = null,
        [FromQuery] int maxItems = 10,
        CancellationToken cancellationToken = default
    )
    {
        // Para simplificar, vamos passar lista vazia de categorias por enquanto
        // No futuro, isso pode ser expandido para aceitar GUIDs de categorias
        var categories = new List<Guid>();
        
        var request = new GetCoursesRequest(
            search,
            (CourseDifficulty?)difficulty,
            categories,
            page,
            maxItems
        );
        
        var response = await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Retorna as informações gerais de um curso + lista de módulos
    /// </summary>
    [HttpGet("courses/{id}")]
    public async Task<ActionResult<GetCourseDetailsResponse>> GetCourseById(
        [FromRoute] Guid id,
        [FromQuery] Guid? userId = null,
        CancellationToken cancellationToken = default
    )
    {
        var response = await _mediator.Send(new GetCourseDetailsRequest(id, userId), cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Matricular usuário em um curso (self-enrollment ou por superior hierárquico)
    /// </summary>
    [HttpPost("{courseId}/enroll")]
    public async Task<ActionResult<EnrollCourseResponse>> Enroll(
        [FromRoute] Guid courseId,
        [FromBody] EnrollCourseRequest request,
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