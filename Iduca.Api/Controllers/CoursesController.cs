using Iduca.Api.Enums;
using Iduca.Application.Features.Companies.Create;
using Iduca.Application.Features.Courses.Create;
using Iduca.Application.Features.Courses.Delete;
using Iduca.Application.Features.Courses.Enroll;
using Iduca.Application.Features.Courses.Get;
using Iduca.Application.Features.Courses.GetByQuery;
using Iduca.Application.Features.Courses.Update;
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
        [FromQuery] CourseDifficulty? Difficulty,
        [FromQuery] int Page = 1,
        [FromQuery] int MaxItems = 10,
        CancellationToken cancellationToken = default
    )
    {
        if (Page < 1 || MaxItems < 1)
            return BadRequest("Page and MaxItems must be greater than 0.");

        // Para simplificar, vamos deixar Categories como lista vazia por enquanto
        var response = await mediator.Send(new GetCoursesRequest(Name, Difficulty, new List<Guid>(), Page, MaxItems), cancellationToken);

        return Ok(response);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<GetCourseResponse>> GetById(
        [FromRoute] Guid id, CancellationToken cancellationToken
    )
    {
        var response = await mediator.Send(new GetCourseRequest(id), cancellationToken);

        return Ok(response);
    }

    [HttpPatch]
    public async Task<ActionResult<UpdateCourseRequest>> Update(
        [FromBody] UpdateCourseRequest request, CancellationToken cancellationToken
    )
    {
        var response = await mediator.Send(request, cancellationToken);

        return Ok(response);
    }

    [HttpPost]
    [Route("{courseId}/enroll")]
    public async Task<ActionResult<EnrollCourseResponse>> Enroll(
        [FromRoute] Guid courseId,
        [FromBody] EnrollCourseRequest request,
        CancellationToken cancellationToken
    )
    {
        // Verificar se o courseId da rota coincide com o do body
        if (courseId != request.CourseId)
            return BadRequest("CourseId na rota deve coincidir com o CourseId no body.");

        var response = await mediator.Send(request, cancellationToken);
        return Ok(response);
    }
}