using Iduca.Api.Attributes;
using Iduca.Application.Common.Services;
using Iduca.Application.Features.Courses.Enroll;
using Iduca.Application.Features.Manager.GetDashboard;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iduca.Api.Controllers;

[ApiController]
[Route("api/manager")]
[CustomAuthorize]
public class ManagerController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IHierarchyService _hierarchyService;

    public ManagerController(IMediator mediator, IHierarchyService hierarchyService)
    {
        _mediator = mediator;
        _hierarchyService = hierarchyService;
    }

    /// <summary>
    /// Dashboard principal para managers
    /// REGRA: Apenas dados dos subordinados diretos e indiretos do manager logado
    /// </summary>
    [HttpGet("dashboard")]
    public async Task<ActionResult<GetManagerDashboardResponse>> GetDashboard(CancellationToken cancellationToken = default)
    {
        var currentUserId = GetCurrentUserId();
        
        var request = new GetManagerDashboardRequest(currentUserId);
        var response = await _mediator.Send(request, cancellationToken);
        
        return Ok(response);
    }

    /// <summary>
    /// Retorna a lista dos colaboradores do time do manager logado
    /// </summary>
    [HttpGet("team")]
    public async Task<ActionResult> GetTeam(CancellationToken cancellationToken = default)
    {
        var currentUserId = GetCurrentUserId();
        
        // Obter subordinados diretos e indiretos
        var allSubordinates = await _hierarchyService.GetSubordinatesAsync(currentUserId, true, cancellationToken);
        var directSubordinates = await _hierarchyService.GetSubordinatesAsync(currentUserId, false, cancellationToken);
        
        if (!allSubordinates.Any())
        {
            return Ok(new { message = "Você não possui subordinados.", team = new List<object>() });
        }

        var teamResponse = allSubordinates.Select(subordinate => new
        {
            id = subordinate.Id,
            name = subordinate.Name,
            email = subordinate.Email,
            identity = subordinate.Identity,
            isAdmin = subordinate.IsAdmin,
            isDirect = directSubordinates.Any(d => d.Id == subordinate.Id),
            responsibleId = subordinate.ResponsibleId
        }).ToArray();

        return Ok(new
        {
            totalSubordinates = allSubordinates.Count,
            directSubordinates = directSubordinates.Count,
            team = teamResponse
        });
    }

    /// <summary>
    /// Retorna a lista de cursos com status de inscrição de um colaborador específico
    /// REGRA: Manager só pode ver status de seus subordinados diretos e indiretos
    /// </summary>
    [HttpGet("courses-status")]
    public async Task<ActionResult> GetCoursesStatus([FromQuery] Guid employeeId, CancellationToken cancellationToken = default)
    {
        var currentUserId = GetCurrentUserId();
        
        // Verificar se o usuário atual pode acessar os dados do funcionário
        if (!await _hierarchyService.CanUserAccessDataAsync(currentUserId, employeeId, cancellationToken))
        {
            return Forbid("Você não tem permissão para acessar os dados deste funcionário. Apenas superiores diretos ou indiretos podem acessar.");
        }
        
        // TODO: Implementar busca real de cursos do funcionário
        var mockResponse = new object[]
        {
            new
            {
                courseId = Guid.NewGuid(),
                title = "Banco de Dados",
                status = 1, // 1 - Completo, 2 - Em progresso, 3 - Não iniciado
                employeeId = employeeId,
                managerCanView = true,
                enrollmentDate = DateTime.UtcNow.AddDays(-30),
                completionDate = DateTime.UtcNow.AddDays(-5),
                progress = 100
            },
            new
            {
                courseId = Guid.NewGuid(),
                title = "Git e GitHub",
                status = 3,
                employeeId = employeeId,
                managerCanView = true,
                enrollmentDate = DateTime.UtcNow.AddDays(-10),
                completionDate = (DateTime?)null,
                progress = 0
            }
        };

        return Ok(new
        {
            employeeId = employeeId,
            courses = mockResponse
        });
    }

    /// <summary>
    /// Inscreve um colaborador em um curso
    /// </summary>
    [HttpPost("enroll")]
    public async Task<ActionResult> EnrollEmployee([FromBody] EnrollCourseRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Retorna resumo de todos os colaboradores do manager
    /// </summary>
    [HttpGet("employeesSummary")]
    public async Task<ActionResult> GetEmployeesSummary(CancellationToken cancellationToken = default)
    {
        var mockResponse = new[]
        {
            new
            {
                id = 21,
                name = "Ana Costa",
                email = "ana@empresa.com",
                coursesCompleted = 4,
                coursesInProgress = 2,
                averageScore = 87,
                topCategory = "Programação",
                isManager = false
            },
            new
            {
                id = 22,
                name = "João Lima",
                email = "joao@empresa.com",
                coursesCompleted = 1,
                coursesInProgress = 1,
                averageScore = 59,
                topCategory = "DevOps",
                isManager = false
            }
        };

        return Ok(mockResponse);
    }

    /// <summary>
    /// Dashboard detalhado de um colaborador específico
    /// REGRA: Manager só pode ver dashboard de seus subordinados diretos e indiretos
    /// </summary>
    [HttpGet("employee/{id}/dashboard")]
    public async Task<ActionResult> GetEmployeeDashboard(Guid id, CancellationToken cancellationToken = default)
    {
        var currentUserId = GetCurrentUserId();
        
        // Verificar se o usuário atual pode acessar os dados do funcionário
        if (!await _hierarchyService.CanUserAccessDataAsync(currentUserId, id, cancellationToken))
        {
            return Forbid("Você não tem permissão para acessar o dashboard deste funcionário. Apenas superiores diretos ou indiretos podem acessar.");
        }
        
        // TODO: Implementar busca real de dados do funcionário
        var mockResponse = new
        {
            employeeId = id,
            name = "Funcionário Exemplo",
            email = "funcionario@empresa.com",
            competencies = new[]
            {
                new { category = "Programação", competenceLevel = 85 },
                new { category = "UX/UI", competenceLevel = 60 },
                new { category = "Gestão", competenceLevel = 45 }
            },
            courses = new
            {
                completed = new[]
                {
                    new
                    {
                        title = "Banco de Dados",
                        category = "Programação",
                        difficulty = 2,
                        score = 92,
                        completedAt = DateTime.UtcNow.AddDays(-10)
                    }
                },
                inProgress = new[]
                {
                    new
                    {
                        title = "Gestão de Projetos",
                        category = "Gestão",
                        difficulty = 2,
                        progress = 40
                    }
                },
                notStarted = new[]
                {
                    new
                    {
                        title = "Introdução ao Figma",
                        category = "UX/UI",
                        difficulty = 1
                    }
                }
            },
            averageScore = 87,
            totalCourses = 7,
            coursesCompleted = 4
        };

        return Ok(mockResponse);
    }

    /// <summary>
    /// Gera um relatório com os dados dos colaboradores
    /// </summary>
    [HttpGet("export")]
    public async Task<ActionResult> ExportReport([FromQuery] string format = "pdf", [FromQuery] Guid? teamId = null, CancellationToken cancellationToken = default)
    {
        // TODO: Implementar geração de relatório PDF/Excel
        return Ok(new { message = "Relatório gerado com sucesso", downloadUrl = "/downloads/relatorio.pdf" });
    }

    /// <summary>
    /// Cria um novo colaborador no sistema, vinculado ao time do manager autenticado
    /// </summary>
    [HttpPost("employees")]
    public async Task<ActionResult> CreateEmployee([FromBody] CreateEmployeeRequest request, CancellationToken cancellationToken = default)
    {
        // TODO: Implementar CreateEmployeeRequest e Handler
        return Ok(new { message = "Employee created successfully", employeeId = 45 });
    }
}

// Classes temporárias para os requests
public class EnrollEmployeeRequest
{
    public Guid EmployeeId { get; set; }
    public Guid CourseId { get; set; }
}

public class CreateEmployeeRequest
{
    public string Name { get; set; } = string.Empty;
    public string Identity { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool IsManager { get; set; }
    public Guid CompanyId { get; set; }
}
