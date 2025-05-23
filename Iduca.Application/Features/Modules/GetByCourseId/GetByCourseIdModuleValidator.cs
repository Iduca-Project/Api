using FluentValidation;

namespace Iduca.Application.Features.Modules.Create;

public class GetByCourseIdModuleValidator : AbstractValidator<GetByCourseIdModuleRequest>
{
    public GetByCourseIdModuleValidator()
    {
        RuleFor(m => m.CourseId)
            .NotEmpty();
    }
}