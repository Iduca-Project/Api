
using AutoMapper;
using Iduca.Application.Common.Exceptions;
using Iduca.Application.Repository;
using Iduca.Application.Repository.ExerciseRepository;
using MediatR;

namespace Iduca.Application.Features.Exercises.GetExerciseDetails;

public class GetExerciseDetailsExercise(
    IUnitOfWork unitOfWork,
    IExerciseRepository exerciseRepository,
    IMapper mapper
) : IRequestHandler<GetExerciseDetailsExerciseRequest, GetExerciseDetailsExerciseResponse>
{
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly IExerciseRepository exerciseRepository = exerciseRepository;
    private readonly IMapper mapper = mapper;

    public async Task<GetExerciseDetailsExerciseResponse> Handle(GetExerciseDetailsExerciseRequest request, CancellationToken cancellationToken)
    {

        var findExercise = await exerciseRepository.GetById(request.Id, cancellationToken)
            ?? throw new NotFoundException("Exercicio não encontrado");

        var exercise = new GetExerciseDetailsExerciseResponse
        (
            findExercise.Id,
            findExercise.Title,
            findExercise.Questions.Select(q =>
            new QuestionProps
            (
                q.Id,
                q.Title,
                q.Alternatives.Select(a =>
                new AlternativeProps
                (   
                    a.Id,
                    a.Description,
                    a.IsCorrect
                )).ToList()
            )).ToList()
        );
        return exercise;
    }
}
