
using MediatR;

namespace Iduca.Application.Features.Exercises.GetExerciseDetails;

public sealed record GetExerciseDetailsExerciseRequest(
    Guid Id
) : IRequest<GetExerciseDetailsExerciseResponse>;
