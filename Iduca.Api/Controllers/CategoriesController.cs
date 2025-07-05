using Iduca.Api.Enums;
using Iduca.Api.Attributes;
using Iduca.Application.Features.Categories.Create;
using Iduca.Application.Features.Categories.DeleteById;
using Iduca.Application.Features.Categories.GetByName;
using Iduca.Application.Features.Categories.Get;
using Iduca.Application.Features.Categories.GetAll;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Iduca.Api.Controllers;

[ApiController]
[Route(APIRoutes.Categories)]
[CustomAuthorize] // Requer autenticação para todas as rotas
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Retorna a lista de categorias disponíveis para o usuário escolher
    /// </summary>
    [HttpGet]
    public async Task<ActionResult> GetCategoriesAsync([FromQuery] GetAllCategoriesRequest request, CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Buscar categorias por nome similar
    /// </summary>
    [HttpGet("search")]
    public async Task<ActionResult<CreateCategoryResponse>> GetBySimilarName(
        [FromQuery] string Name, CancellationToken cancellationToken
    )
    {
        var response = await _mediator.Send(new GetByNameCategoryRequest(Name), cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Obter categoria por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<GetCategoryResponse>> GetById(
        [FromRoute] Guid id, CancellationToken cancellationToken
    )
    {
        var response = await _mediator.Send(new GetCategoryRequest(id), cancellationToken);
        return Ok(response);
    }

    /// <summary>
    /// Listar todas as categorias
    /// </summary>
    [HttpGet("all")]
    public async Task<ActionResult<List<GetAllCategoriesResponse>>> GetAll(
        CancellationToken cancellationToken
    )
    {
        var response = await _mediator.Send(new GetAllCategoriesRequest(), cancellationToken);
        return Ok(response);
    }
}
