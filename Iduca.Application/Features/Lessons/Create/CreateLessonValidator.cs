
using FluentValidation;

namespace Iduca.Application.Features.Lessons.Create;

public class CreateLessonValidator : AbstractValidator<CreateLessonRequest>
{
    public CreateLessonValidator()
    {
        //RuleFor(m => m.[propertie])
        //    .NotEmpty()
        //    .MaximumLength(64)
        //    .MinimumLength(8);
    }
}
