using Iduca.Api.Attributes;
using Iduca.Application.Common.Services;
using Iduca.Application.Features.Analytics.CategoryStats;
using Iduca.Application.Features.Analytics.CompanyStats;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iduca.Api.Controllers;

[ApiController]
[Route("api/analytics")]
[CustomAuthorize]
public class AnalyticsController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IHierarchyService _hierarchyService;

    public AnalyticsController(IMediator mediator, IHierarchyService hierarchyService)
    {
        _mediator = mediator;
        _hierarchyService = hierarchyService;
    }

    /// <summary>
    /// Obter estatísticas detalhadas de uma empresa
    /// Apenas administradores ou usuários com subordinados podem acessar
    /// </summary>
    /// <param name="companyId">ID da empresa</param>
    /// <param name="startDate">Data inicial para filtro (opcional)</param>
    /// <param name="endDate">Data final para filtro (opcional)</param>
    /// <returns>Estatísticas completas da empresa</returns>
    [HttpGet("companies/{companyId:guid}")]
    public async Task<ActionResult<GetCompanyStatsResponse>> GetCompanyStats(
        Guid companyId,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = GetCurrentUserId();
        var currentUserCompanyId = GetCurrentUserCompanyId();

        // Verificar se o usuário está tentando acessar dados de sua própria empresa
        if (companyId != currentUserCompanyId)
        {
            return Forbid("Você só pode acessar estatísticas de sua própria empresa.");
        }

        // Verificar se o usuário é admin ou tem subordinados (é responsável por alguém)
        var isAdmin = IsCurrentUserAdmin();
        if (!isAdmin)
        {
            var subordinates = await _hierarchyService.GetAllSubordinatesAsync(currentUserId, false, cancellationToken);
            if (!subordinates.Any())
            {
                return Forbid("Você precisa ser administrador ou responsável por outros usuários para acessar estatísticas da empresa.");
            }
        }

        var request = new GetCompanyStatsRequest(companyId, startDate, endDate);
        var response = await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Obter estatísticas detalhadas de uma categoria
    /// Apenas administradores ou usuários com subordinados podem acessar
    /// </summary>
    /// <param name="categoryId">ID da categoria</param>
    /// <param name="startDate">Data inicial para filtro (opcional)</param>
    /// <param name="endDate">Data final para filtro (opcional)</param>
    /// <returns>Estatísticas completas da categoria</returns>
    [HttpGet("categories/{categoryId:guid}")]
    public async Task<ActionResult<GetCategoryStatsResponse>> GetCategoryStats(
        Guid categoryId,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = GetCurrentUserId();

        // Verificar se o usuário é admin ou tem subordinados (é responsável por alguém)
        var isAdmin = IsCurrentUserAdmin();
        if (!isAdmin)
        {
            var subordinates = await _hierarchyService.GetAllSubordinatesAsync(currentUserId, false, cancellationToken);
            if (!subordinates.Any())
            {
                return Forbid("Você precisa ser administrador ou responsável por outros usuários para acessar estatísticas de categorias.");
            }
        }

        var request = new GetCategoryStatsRequest(categoryId, startDate, endDate);
        var response = await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Obter estatísticas de um usuário específico
    /// Apenas o próprio usuário, seu superior ou administradores podem acessar
    /// </summary>
    /// <param name="userId">ID do usuário</param>
    /// <param name="startDate">Data inicial para filtro (opcional)</param>
    /// <param name="endDate">Data final para filtro (opcional)</param>
    /// <returns>Estatísticas do usuário</returns>
    [HttpGet("users/{userId:guid}")]
    public async Task<ActionResult> GetUserStats(
        Guid userId,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = GetCurrentUserId();

        // Verificar se o usuário atual pode acessar os dados do usuário alvo
        if (!await _hierarchyService.CanUserAccessDataAsync(currentUserId, userId, cancellationToken))
        {
            return Forbid("Você não tem permissão para acessar as estatísticas deste usuário.");
        }

        // Mock response - implementar lógica real conforme necessário
        return Ok(new
        {
            userId = userId,
            period = new { startDate, endDate },
            stats = new
            {
                coursesCompleted = 5,
                coursesInProgress = 2,
                totalHoursStudied = 25.5,
                averageScore = 85.7,
                lastActivity = DateTime.UtcNow.AddDays(-1)
            }
        });
    }

    /// <summary>
    /// Obter estatísticas dos subordinados do usuário atual
    /// </summary>
    /// <param name="includeIndirect">Incluir subordinados indiretos</param>
    /// <param name="startDate">Data inicial para filtro (opcional)</param>
    /// <param name="endDate">Data final para filtro (opcional)</param>
    /// <returns>Estatísticas dos subordinados</returns>
    [HttpGet("my-team")]
    public async Task<ActionResult> GetMyTeamStats(
        [FromQuery] bool includeIndirect = false,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null,
        CancellationToken cancellationToken = default)
    {
        var currentUserId = GetCurrentUserId();

        // Obter subordinados
        var subordinates = await _hierarchyService.GetSubordinatesAsync(currentUserId, includeIndirect, cancellationToken);

        if (!subordinates.Any())
        {
            return Ok(new
            {
                message = "Você não possui subordinados.",
                teamStats = new List<object>()
            });
        }

        // Mock response - implementar lógica real conforme necessário
        var teamStats = subordinates.Select(subordinate => new
        {
            user = new
            {
                id = subordinate.Id,
                name = subordinate.Name,
                email = subordinate.Email
            },
            stats = new
            {
                coursesCompleted = Random.Shared.Next(1, 10),
                coursesInProgress = Random.Shared.Next(0, 5),
                totalHoursStudied = Math.Round(Random.Shared.NextDouble() * 50, 1),
                averageScore = Math.Round(Random.Shared.NextDouble() * 40 + 60, 1)
            }
        }).ToList();

        return Ok(new
        {
            teamSize = subordinates.Count,
            includeIndirect = includeIndirect,
            period = new { startDate, endDate },
            teamStats = teamStats
        });
    }
}
