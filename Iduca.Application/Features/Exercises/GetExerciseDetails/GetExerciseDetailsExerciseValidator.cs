
using FluentValidation;

namespace Iduca.Application.Features.Exercises.GetExerciseDetails;

public class GetExerciseDetailsExerciseValidator : AbstractValidator<GetExerciseDetailsExerciseRequest>
{
    public GetExerciseDetailsExerciseValidator()
    {
        //RuleFor(m => m.[propertie])
        //    .NotEmpty()
        //    .MaximumLength(64)
        //    .MinimumLength(8);
    }
}
