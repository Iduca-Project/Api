
using FluentValidation;

namespace Iduca.Application.Features.Lessons.Create;

public class CreateLessonValidator : AbstractValidator<CreateLessonRequest>
{
    public CreateLessonValidator()
    {
        RuleFor(l => l.Title)
           .NotEmpty()
           .MaximumLength(64);

        RuleFor(l => l.Description)
           .NotEmpty()
           .MaximumLength(511);
    }
}
