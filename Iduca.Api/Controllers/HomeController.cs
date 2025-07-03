using Iduca.Api.Attributes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iduca.Api.Controllers;

[ApiController]
[Route("api/home")]
[CustomAuthorize] // Requer autenticação para todas as rotas
public class HomeController(IMediator mediator) : ControllerBase
{
    private readonly IMediator mediator = mediator;

    /// <summary>
    /// Retorna o progresso geral do usuário nos cursos
    /// </summary>
    [HttpGet("progress")]
    public async Task<ActionResult> GetProgress(CancellationToken cancellationToken = default)
    {
        // TODO: Implementar GetProgressRequest e Handler
        // Por enquanto, retorna uma resposta mockada
        var mockResponse = new
        {
            username = "João da Silva",
            isManager = false,
            isAdmin = false,
            totalCourses = 10,
            ongoingCourses = 4,
            completeCourses = 6,
            percenteGeneral = 60
        };

        return Ok(mockResponse);
    }

    /// <summary>
    /// Retorna até 8 cursos que o usuário está fazendo atualmente
    /// </summary>
    [HttpGet("coursesInProgress")]
    public async Task<ActionResult> GetCoursesInProgress(CancellationToken cancellationToken = default)
    {
        // TODO: Implementar GetCoursesInProgressRequest e Handler
        var mockResponse = new[]
        {
            new
            {
                id = 1,
                title = "Lógica de Programação",
                image = "https://cdn.exemplo.com/curso1.png",
                progress = 40,
                description = "Python é uma das linguagens que mais cresce no Mercado de Trabalho e atualmente uma das mais usadas e requisitadas pelas empresas.",
                rating = 4.8,
                participants = 157,
                difficulty = 2,
                category = "Programação"
            },
            new
            {
                id = 2,
                title = "Banco de Dados",
                image = "https://cdn.exemplo.com/curso2.png",
                progress = 75,
                description = "Python é uma das linguagens que mais cresce no Mercado de Trabalho e atualmente uma das mais usadas e requisitadas pelas empresas.",
                rating = 4.8,
                participants = 157,
                difficulty = 2,
                category = "Programação"
            }
        };

        return Ok(mockResponse);
    }

    /// <summary>
    /// Retorna os lembretes do usuário e as datas de prazos de atividades/provas
    /// </summary>
    [HttpGet("calendar")]
    public async Task<ActionResult> GetCalendar(CancellationToken cancellationToken = default)
    {
        // TODO: Implementar GetCalendarRequest e Handler
        var mockResponse = new[]
        {
            new
            {
                date = "2025-05-15",
                type = 1,
                description = "Estudar para a prova de C#"
            },
            new
            {
                date = "2025-05-17",
                type = 2,
                description = "Prazo final da atividade de Banco de Dados"
            },
            new
            {
                date = "2025-05-20",
                type = 3,
                description = "Prova de Node.js"
            }
        };

        return Ok(mockResponse);
    }
}
