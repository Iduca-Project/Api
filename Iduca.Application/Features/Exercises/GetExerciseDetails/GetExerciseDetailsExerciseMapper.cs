
using AutoMapper;
using Iduca.Domain.Models;

namespace Iduca.Application.Features.Exercises.GetExerciseDetails;

public class GetExerciseDetailsExerciseMapper : Profile
{
    public GetExerciseDetailsExerciseMapper()
    {
        CreateMap<GetExerciseDetailsExerciseRequest, Exercise>();
        CreateMap<Exercise, GetExerciseDetailsExerciseResponse>();
    }
}
