using Iduca.Api.Enums;
using Iduca.Application.Features.Companies.Create;
using Iduca.Application.Features.Courses.Create;
using Iduca.Application.Features.Courses.Delete;
using Iduca.Application.Features.Courses.GetByQuery;
using Iduca.Domain.Common.Enums;
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
        [FromBody] List<Guid>? Categories,
        [FromQuery] int Page,
        [FromQuery] int MaxItens,
        [FromQuery] CourseDifficulty? Difficulty,
        CancellationToken cancellationToken
    )
    {
        if (Page < 1 && MaxItens < 1)
            return BadRequest("Page and MaxItens must be greater than 0.");

        var response = await mediator.Send(new GetCoursesRequest(Name, Difficulty, Categories, Page, MaxItens), cancellationToken);

        return Ok(response);
    }
}