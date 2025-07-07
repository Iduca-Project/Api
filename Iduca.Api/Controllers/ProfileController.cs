using Iduca.Api.Attributes;
using Iduca.Application.Features.User.Get;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Iduca.Api.Controllers;

[ApiController]
[Route("api")]
[CustomAuthorize] // Requer autenticação para todas as rotas
public class ProfileController(IMediator mediator) : BaseController
{
    private readonly IMediator mediator = mediator;

    /// <summary>
    /// Retorna todas as informações do usuário logado
    /// </summary>
    [HttpGet("profile")]
    public async Task<ActionResult> GetProfileAsync(CancellationToken cancellationToken = default)
    {
        var userId = GetCurrentUserId();

        var response = await mediator.Send(new GetUserRequest(userId), cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Retorna a imagem do certificado de um curso finalizado
    /// </summary>
    [HttpGet("certificate/{id}/image")]
    public ActionResult GetCertificateImage(int id, CancellationToken cancellationToken = default)
    {
        // TODO: Implementar GetCertificateImageRequest e Handler
        return Ok(new { response = "certificado.png" });
    }

    /// <summary>
    /// Retorna o PDF do certificado para download
    /// </summary>
    [HttpGet("certificate/{id}/pdf")]
    public ActionResult GetCertificatePdf(int id, CancellationToken cancellationToken = default)
    {
        // TODO: Implementar GetCertificatePdfRequest e Handler
        return Ok(new { response = "certificado.pdf" });
    }

    /// <summary>
    /// Retorna a lista de interesses disponíveis para o usuário escolher (máximo de 5)
    /// </summary>
    [HttpGet("interests")]
    [AllowAnonymous] // Esta rota não precisa de autenticação conforme README
    public ActionResult GetInterests(CancellationToken cancellationToken = default)
    {
        // TODO: Implementar GetInterestsRequest e Handler ou buscar direto das categorias
        var mockResponse = new[]
        {
            new { id = 1, name = "Programação" },
            new { id = 2, name = "UX/UI" },
            new { id = 3, name = "DevOps" },
            new { id = 4, name = "Gestão" },
            new { id = 5, name = "Banco de Dados" },
            new { id = 6, name = "Inteligência Artificial" },
            new { id = 7, name = "Mecânica" }
        };

        return Ok(mockResponse);
    }

    /// <summary>
    /// Permite que o usuário edite sua foto de perfil e/ou seus interesses (até 5)
    /// </summary>
    [HttpPut("profile")]
    public ActionResult UpdateProfile([FromBody] UpdateProfileRequest request, CancellationToken cancellationToken = default)
    {
        // TODO: Implementar UpdateProfileRequest e Handler
        
        // Validação básica
        if (request.Interests != null && request.Interests.Length > 5)
        {
            return BadRequest(new { message = "Máximo de 5 interesses permitidos" });
        }

        return Ok(new { response = true });
    }
}

// Classe temporária para o request
public class UpdateProfileRequest
{
    public string? PhotoUser { get; set; }
    public int[]? Interests { get; set; }
}
