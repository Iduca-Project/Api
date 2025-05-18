using Iduca.Api.Enums;
using Iduca.Application.Features.Companies.Create;
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
}