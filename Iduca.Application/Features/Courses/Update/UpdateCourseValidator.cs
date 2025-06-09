using FluentValidation;

namespace Iduca.Application.Features.Courses.Update;

public class UpdateCourseValidator : AbstractValidator<UpdateCourseRequest>
{
    public UpdateCourseValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(50);

        RuleFor(c => c.Description)
            .NotEmpty()
            .MinimumLength(32)
            .MaximumLength(511);
    }
}