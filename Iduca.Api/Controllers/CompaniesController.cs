using Iduca.Api.Enums;
using Iduca.Application.Features.Companies.Create;
using Iduca.Application.Features.Companies.Delete;
using Iduca.Application.Features.Companies.Get;
using Iduca.Application.Features.Companies.GetAll;
using Iduca.Application.Features.Companies.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iduca.Api.Controllers;

[ApiController]
[Route(APIRoutes.Companies)]
public class CompaniesController(IMediator mediator) : ControllerBase
{
    private readonly IMediator mediator = mediator;

    [HttpPost]
    public async Task<ActionResult<CreateCompanyResponse>> Create(
        CreateCompanyRequest request, CancellationToken cancellationToken
    )
    {
        var response = await mediator.Send(request, cancellationToken);

        return Created(APIRoutes.Companies, response);
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<ActionResult> Delete(
        [FromRoute] Guid id, CancellationToken cancellationToken
    )
    {
        await mediator.Send(new DeleteCompanyRequest(id), cancellationToken);

        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<GetCompanyResponse>> Get(
        GetCompanyRequest request, CancellationToken cancellationToken
    )
    {
        var response = await mediator.Send(new GetCompanyRequest(request.Name), cancellationToken);

        return Ok(response);
    }

    [HttpGet]
    [Route("all")]
    public async Task<ActionResult<GetAllCompanyResponse>> GetAll(
        CancellationToken cancellationToken
    )
    {
        var response = await mediator.Send(new GetAllCompanyRequest(), cancellationToken);

        return Ok(response);
    }

    [HttpPatch]
    [Route("{id}")]
    public async Task<ActionResult<UpdateCompanyResponse>> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateCompanyRequestProps body,
        CancellationToken cancellationToken
    )
    {
        var request = new UpdateCompanyRequest(id, body);
        var response = await mediator.Send(request, cancellationToken);

        return Ok(response);
    }
}