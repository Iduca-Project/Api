using Iduca.Api.Enums;
using Iduca.Api.Attributes;
using Iduca.Application.Features.Categories.Create;
using Iduca.Application.Features.Categories.DeleteById;
using Iduca.Application.Features.Categories.GetByName;
using Iduca.Application.Features.Categories.Get;
using Iduca.Application.Features.Categories.GetAll;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iduca.Api.Controllers;

[ApiController]
[Route(APIRoutes.Categories)]
[CustomAuthorize] // Todas as rotas de categoria precisam de autenticação
public class CategoriesController(IMediator mediator) : ControllerBase
{
    private readonly IMediator mediator = mediator;

    [HttpPost]
    public async Task<ActionResult<CreateCategoryResponse>> Create(
        [FromBody] CreateCategoryRequest request, CancellationToken cancellationToken
    )
    {
        var response = await mediator.Send(request, cancellationToken);
        return Created(APIRoutes.Categories, response);
    }

    [HttpGet]
    [Route("search")]
    public async Task<ActionResult<CreateCategoryResponse>> GetBySimilarName(
        [FromQuery] string Name, CancellationToken cancellationToken
    )
    {
        var response = await mediator.Send(new GetByNameCategoryRequest(Name), cancellationToken);
        return Ok(response);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<GetCategoryResponse>> GetById(
        [FromRoute] Guid id, CancellationToken cancellationToken
    )
    {
        var response = await mediator.Send(new GetCategoryRequest(id), cancellationToken);
        return Ok(response);
    }

    [HttpGet]
    [Route("all")]
    public async Task<ActionResult<List<GetAllCategoriesResponse>>> GetAll(
        CancellationToken cancellationToken
    )
    {
        var response = await mediator.Send(new GetAllCategoriesRequest(), cancellationToken);
        return Ok(response);
    }

    [HttpDelete]
    [Route("{Id}")]
    public async Task<ActionResult<DeleteByIdCategoryResponse>> DeleteById(
        [FromRoute] Guid Id, CancellationToken cancellationToken
    )
    {
        await mediator.Send(new DeleteByIdCategoryRequest(Id), cancellationToken);
        return Ok();
    }
}
