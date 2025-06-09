
using FluentValidation;

namespace Iduca.Application.Features.Lessons.GetByModuleId;

public class GetByModuleIdLessonValidator : AbstractValidator<GetByModuleIdLessonRequest>
{
    public GetByModuleIdLessonValidator()
    {
        RuleFor(l => l.ModuleId)
           .NotEmpty();
    }
}
