using Iduca.Api.Enums;
using Iduca.Api.Attributes;
using Iduca.Application.Common.Services;
using Iduca.Application.Features.Courses.GetByQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Iduca.Application.Features.Courses.GetCourseOfUser;
using Iduca.Application.Features.Exercises.GetExerciseDetails;

namespace Iduca.Api.Controllers;

[ApiController]
[Route(APIRoutes.Exercises)]
[CustomAuthorize] // Requer autenticação para todas as rotas
public class ExercisesController
(
    IMediator mediator
) : BaseController
{
    private readonly IMediator _mediator = mediator;


    /// <summary>
    /// Retorna a lista paginada de cursos com suporte a busca e filtros
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<GetCoursesResponse>> GetDeatils(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default
    )
    {
        var response = await _mediator.Send(new GetExerciseDetailsExerciseRequest(id), cancellationToken);
        return Ok(response);
    }

}