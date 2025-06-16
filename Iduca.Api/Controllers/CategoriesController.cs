using Iduca.Api.Enums;
using Iduca.Application.Features.Categories.Create;
using Iduca.Application.Features.Categories.DeleteById;
using Iduca.Application.Features.Categories.GetByName;
using Iduca.Application.Features.Categories.GetAll;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iduca.Api.Controllers;

[ApiController]
[Route(APIRoutes.Categories)]
public class CategoriesController(IMediator mediator) : ControllerBase
{
    private readonly IMediator mediator = mediator;

    [HttpPost]
    public async Task<ActionResult<CreateCategoryResponse>> Create(
        CreateCategoryRequest request, CancellationToken cancellationToken
    )
    {
        var response = await mediator.Send(request, cancellationToken);
        return Created(APIRoutes.Categories, response);
    }
    [HttpGet]
    public async Task<ActionResult<CreateCategoryResponse>> GetBySimilarName(
        [FromQuery] string Name, CancellationToken cancellationToken
    )
    {
        var response = await mediator.Send(new GetByNameCategoryRequest(Name), cancellationToken);
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

    [HttpGet]
    [Route("all")]
    public async Task<ActionResult<GetAllCategoryResponse>> GetByQuery(
        GetAllCategoryRequest request, CancellationToken cancellationToken
    )
    {
        await mediator.Send(request, cancellationToken);
        return Ok();
    }
}
