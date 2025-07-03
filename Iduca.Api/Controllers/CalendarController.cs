using Iduca.Api.Attributes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iduca.Api.Controllers;

[ApiController]
[Route("api/calendar")]
[CustomAuthorize] // Requer autenticação para todas as rotas
public class CalendarController(IMediator mediator) : ControllerBase
{
    private readonly IMediator mediator = mediator;

    /// <summary>
    /// Retorna todos os eventos do usuário em até 1 ano (6 meses antes e 6 meses depois do dia atual)
    /// </summary>
    [HttpGet]
    public ActionResult GetCalendar(CancellationToken cancellationToken = default)
    {
        // TODO: Implementar GetCalendarEventsRequest e Handler
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

    /// <summary>
    /// Retorna os eventos dos próximos 7 dias (lembretes + prazos + provas)
    /// </summary>
    [HttpGet("next")]
    public ActionResult GetNextEvents(CancellationToken cancellationToken = default)
    {
        // TODO: Implementar GetNextEventsRequest e Handler
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
            }
        };

        return Ok(mockResponse);
    }

    /// <summary>
    /// Permite ao usuário adicionar um lembrete pessoal
    /// </summary>
    [HttpPost("reminder")]
    public ActionResult AddReminder([FromBody] AddReminderRequest request, CancellationToken cancellationToken = default)
    {
        // TODO: Implementar AddReminderRequest e Handler
        return Ok(new { response = true });
    }
}

// Classe temporária para o request
public class AddReminderRequest
{
    public string Title { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
}
