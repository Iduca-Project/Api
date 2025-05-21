using Iduca.Api.Enums;
using Iduca.Application.Features.Companies.Create;
using Iduca.Application.Features.Companies.Delete;
using Iduca.Application.Features.Courses.Create;
using Iduca.Application.Features.Courses.Delete;
using Iduca.Application.Features.Courses.GetByQuery;
using Iduca.Domain.Common.Enums;
using Iduca.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iduca.Api.Controllers;

[ApiController]
[Route(APIRoutes.Courses)]
public class CoursesController(IMediator mediator) : ControllerBase
{
    private readonly IMediator mediator = mediator;

    [HttpPost]
    public async Task<ActionResult<CreateCourseResponse>> Create(
        CreateCourseRequest request, CancellationToken cancellationToken
    )
    {
        var response = await mediator.Send(request, cancellationToken);

        return Created(APIRoutes.Courses, response);
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<ActionResult> Delete(
        [FromRoute] Guid id, CancellationToken cancellationToken
    )
    {
        await mediator.Send(new DeleteCourseRequest(id), cancellationToken);

        return NoContent();
    }

    [HttpGet]
    [Route("all")]
    public async Task<ActionResult<List<GetCoursesResponse>>> GetAll(
        [FromQuery] string? Name,
        [FromQuery] CourseDifficulty? Difficulty,
        [FromQuery] Guid? Category,
        CancellationToken cancellationToken
    )
    {
        var response = await mediator.Send(new GetCoursesRequest(Name, Difficulty, Category), cancellationToken);

        return Ok(response);
    }
}