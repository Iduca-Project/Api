using FluentValidation;

namespace Iduca.Application.Features.Courses.GetByQuery;

public class GetCoursesValidator : AbstractValidator<GetCoursesRequest>
{
    public GetCoursesValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(50);
    }
}