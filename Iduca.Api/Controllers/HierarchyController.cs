using Iduca.Api.Attributes;
using Iduca.Api.Enums;
using Iduca.Application.Common.Services;
using Iduca.Application.Features.Courses.Enroll;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iduca.Api.Controllers;

/// <summary>
/// Controller para gerenciar hierarquia de usuários (responsabilidades/subordinados)
/// </summary>
[ApiController]
[Route(APIRoutes.Hierarchy)]
[CustomAuthorize]
public class HierarchyController : BaseController
{
    private readonly IHierarchyService _hierarchyService;
    private readonly IMediator _mediator;

    public HierarchyController(IHierarchyService hierarchyService, IMediator mediator)
    {
        _hierarchyService = hierarchyService;
        _mediator = mediator;
    }

    /// <summary>
    /// Atribuir um responsável para um usuário
    /// </summary>
    /// <param name="userId">ID do usuário que receberá o responsável</param>
    /// <param name="responsibleId">ID do usuário que será o responsável</param>
    [HttpPost("{userId}/assign-responsible/{responsibleId}")]
    [HierarchyAuthorize]
    public async Task<IActionResult> AssignResponsible(Guid userId, Guid responsibleId)
    {
        try
        {
            // Verificar se o usuário atual tem permissão para fazer essa atribuição
            var currentUserId = GetCurrentUserId();
            if (!await _hierarchyService.CanUserAccessDataAsync(currentUserId, userId))
            {
                return Forbid("Você não tem permissão para alterar a hierarquia deste usuário.");
            }

            var result = await _hierarchyService.AssignResponsibleAsync(userId, responsibleId);

            if (!result)
            {
                return BadRequest(new { 
                    message = "Não foi possível atribuir o responsável. Verifique se os usuários existem, são da mesma empresa e se não criaria um ciclo na hierarquia." 
                });
            }

            return Ok(new { 
                message = "Responsável atribuído com sucesso.",
                userId = userId,
                responsibleId = responsibleId
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro interno do servidor.", error = ex.Message });
        }
    }

    /// <summary>
    /// Remover o responsável de um usuário
    /// </summary>
    /// <param name="userId">ID do usuário que terá o responsável removido</param>
    [HttpDelete("{userId}/remove-responsible")]
    [HierarchyAuthorize]
    public async Task<IActionResult> RemoveResponsible(Guid userId)
    {
        try
        {
            // Verificar se o usuário atual tem permissão para fazer essa remoção
            var currentUserId = GetCurrentUserId();
            if (!await _hierarchyService.CanUserAccessDataAsync(currentUserId, userId))
            {
                return Forbid("Você não tem permissão para alterar a hierarquia deste usuário.");
            }

            var result = await _hierarchyService.RemoveResponsibleAsync(userId);

            if (!result)
            {
                return BadRequest(new { message = "Não foi possível remover o responsável. Verifique se o usuário existe." });
            }

            return Ok(new { 
                message = "Responsável removido com sucesso.",
                userId = userId
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro interno do servidor.", error = ex.Message });
        }
    }

    /// <summary>
    /// Obter a árvore hierárquica da empresa
    /// </summary>
    /// <param name="rootUserId">ID do usuário raiz (opcional)</param>
    [HttpGet("tree")]
    public async Task<IActionResult> GetHierarchyTree([FromQuery] Guid? rootUserId = null)
    {
        try
        {
            var companyId = GetCurrentUserCompanyId();
            var tree = await _hierarchyService.GetHierarchyTreeAsync(companyId, rootUserId);

            return Ok(tree);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro interno do servidor.", error = ex.Message });
        }
    }

    /// <summary>
    /// Obter subordinados diretos ou indiretos de um usuário
    /// </summary>
    /// <param name="userId">ID do usuário</param>
    /// <param name="includeIndirect">Incluir subordinados indiretos</param>
    [HttpGet("{userId}/subordinates")]
    public async Task<IActionResult> GetSubordinates(Guid userId, [FromQuery] bool includeIndirect = false)
    {
        try
        {
            // Verificar se o usuário atual tem permissão para ver os subordinados
            var currentUserId = GetCurrentUserId();
            if (!await _hierarchyService.CanUserAccessDataAsync(currentUserId, userId))
            {
                return Forbid("Você não tem permissão para visualizar os subordinados deste usuário.");
            }

            var subordinates = await _hierarchyService.GetSubordinatesAsync(userId, includeIndirect);

            return Ok(new {
                userId = userId,
                includeIndirect = includeIndirect,
                subordinates = subordinates.Select(s => new
                {
                    id = s.Id,
                    name = s.Name,
                    email = s.Email,
                    isAdmin = s.IsAdmin,
                    responsibleId = s.ResponsibleId
                })
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro interno do servidor.", error = ex.Message });
        }
    }

    /// <summary>
    /// Obter cadeia de superiores de um usuário
    /// </summary>
    /// <param name="userId">ID do usuário</param>
    [HttpGet("{userId}/superiors")]
    public async Task<IActionResult> GetSuperiors(Guid userId)
    {
        try
        {
            // Verificar se o usuário atual tem permissão para ver os superiores
            var currentUserId = GetCurrentUserId();
            if (currentUserId != userId && !await _hierarchyService.CanUserAccessDataAsync(currentUserId, userId))
            {
                return Forbid("Você não tem permissão para visualizar os superiores deste usuário.");
            }

            var superiors = await _hierarchyService.GetSuperiorsAsync(userId);

            return Ok(new {
                userId = userId,
                superiors = superiors.Select(s => new
                {
                    id = s.Id,
                    name = s.Name,
                    email = s.Email,
                    isAdmin = s.IsAdmin,
                    responsibleId = s.ResponsibleId
                })
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro interno do servidor.", error = ex.Message });
        }
    }

    /// <summary>
    /// Verificar se um usuário é superior a outro
    /// </summary>
    /// <param name="superiorId">ID do possível superior</param>
    /// <param name="subordinateId">ID do possível subordinado</param>
    [HttpGet("check-hierarchy/{superiorId}/{subordinateId}")]
    public async Task<IActionResult> CheckHierarchy(Guid superiorId, Guid subordinateId)
    {
        try
        {
            var isSuperior = await _hierarchyService.IsUserSuperiorToAsync(superiorId, subordinateId);

            return Ok(new {
                superiorId = superiorId,
                subordinateId = subordinateId,
                isSuperior = isSuperior
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro interno do servidor.", error = ex.Message });
        }
    }

    /// <summary>
    /// Verificar se uma atribuição criaria um ciclo
    /// </summary>
    /// <param name="userId">ID do usuário</param>
    /// <param name="newResponsibleId">ID do novo responsável</param>
    [HttpGet("check-cycle/{userId}/{newResponsibleId}")]
    public async Task<IActionResult> CheckCycle(Guid userId, Guid newResponsibleId)
    {
        try
        {
            var wouldCreateCycle = await _hierarchyService.WouldCreateCycleAsync(userId, newResponsibleId, CancellationToken.None);

            return Ok(new {
                userId = userId,
                newResponsibleId = newResponsibleId,
                wouldCreateCycle = wouldCreateCycle
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro interno do servidor.", error = ex.Message });
        }
    }

    /// <summary>
    /// Obter usuários acessíveis pelo usuário atual
    /// </summary>
    [HttpGet("accessible-users")]
    public async Task<IActionResult> GetAccessibleUsers()
    {
        try
        {
            var currentUserId = GetCurrentUserId();
            var accessibleIds = await _hierarchyService.GetAccessibleUserIdsAsync(currentUserId);

            return Ok(new {
                currentUserId = currentUserId,
                accessibleUserIds = accessibleIds,
                count = accessibleIds.Count
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Erro interno do servidor.", error = ex.Message });
        }
    }

    /// <summary>
    /// Matricular todos os subordinados em um curso
    /// </summary>
    /// <param name="courseId">ID do curso</param>
    /// <param name="includeIndirect">Incluir subordinados indiretos</param>
    // [HttpPost("enroll-team/{courseId}")]
    // public async Task<IActionResult> EnrollTeamInCourse(Guid courseId, [FromQuery] bool includeIndirect = false)
    // {
    //     try
    //     {
    //         var currentUserId = GetCurrentUserId();
            
    //         // Verificar se o usuário tem permissão para matricular no curso
    //         if (!await _hierarchyService.CanUserAccessDataAsync(currentUserId, currentUserId))
    //         {
    //             return Forbid("Você não tem permissão para realizar matrículas em massa.");
    //         }

    //         // Obter subordinados
    //         var subordinates = await _hierarchyService.GetSubordinatesAsync(currentUserId, includeIndirect);
            
    //         if (!subordinates.Any())
    //         {
    //             return BadRequest(new { message = "Você não possui subordinados para matricular." });
    //         }

    //         // Matricular cada subordinado no curso
    //         var successfulEnrollments = new List<object>();
    //         var failedEnrollments = new List<object>();

    //         foreach (var subordinate in subordinates)
    //         {
    //             try
    //             {
    //                 // Usar o mediador para matricular cada usuário
    //                 var enrollRequest = new EnrollCourseRequest(subordinate.Id, courseId);
    //                 var enrollResult = await _mediator.Send(enrollRequest);
                    
    //                 successfulEnrollments.Add(new
    //                 {
    //                     id = subordinate.Id,
    //                     name = subordinate.Name,
    //                     email = subordinate.Email,
    //                     status = "success"
    //                 });
    //             }
    //             catch (Exception ex)
    //             {
    //                 failedEnrollments.Add(new
    //                 {
    //                     id = subordinate.Id,
    //                     name = subordinate.Name,
    //                     email = subordinate.Email,
    //                     status = "failed",
    //                     error = ex.Message
    //                 });
    //             }
    //         }

    //         return Ok(new {
    //             message = $"Processo de matrícula concluído para {subordinates.Count} subordinados no curso {courseId}",
    //             courseId = courseId,
    //             totalProcessed = subordinates.Count,
    //             successfulEnrollments = successfulEnrollments.Count,
    //             failedEnrollments = failedEnrollments.Count,
    //             successful = successfulEnrollments,
    //             failed = failedEnrollments,
    //             includeIndirect = includeIndirect
    //         });
    //     }
    //     catch (Exception ex)
    //     {
    //         return StatusCode(500, new { message = "Erro interno do servidor.", error = ex.Message });
    //     }
    // }
}

/// <summary>
/// Request para atribuir responsabilidade
/// </summary>
public class AssignResponsibilityRequest
{
    public Guid UserId { get; set; }
    public Guid ResponsibleId { get; set; }
}
