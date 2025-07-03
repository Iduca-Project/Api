using Iduca.Api.Attributes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iduca.Api.Controllers;

[ApiController]
[Route("api")]
[CustomAuthorize] // Requer autenticação para todas as rotas
public class TestController(IMediator mediator) : ControllerBase
{
    private readonly IMediator mediator = mediator;

    /// <summary>
    /// Retorna as questões e alternativas de uma prova por ID do curso
    /// </summary>
    [HttpGet("test/{id}")]
    public ActionResult GetTestByCourseId(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default
    )
    {
        // TODO: Implementar GetTestByCourseIdRequest e Handler
        var mockResponse = new
        {
            id = 1,
            title = "Prova Final",
            courseId = 5,
            completed = false,
            content = new object[]
            {
                new
                {
                    id = 1,
                    question = "What is the command to initialize a Git repository?",
                    options = new object[]
                    {
                        new { id = "1", text = "git start", alternative = "a" },
                        new { id = "2", text = "git init", alternative = "b" },
                        new { id = "3", text = "git begin", alternative = "c" }
                    }
                },
                new
                {
                    id = 2,
                    question = "What file tracks your commits?",
                    options = new object[]
                    {
                        new { id = "1", text = ".git/config", alternative = "a" },
                        new { id = "2", text = ".gitignore", alternative = "b" },
                        new { id = "3", text = ".git", alternative = "c" }
                    }
                }
            }
        };

        return Ok(mockResponse);
    }
}
