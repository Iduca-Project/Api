
using FluentValidation;

namespace Iduca.Application.Features.Lessons.GetByModuleByUser;

public class GetByModuleByUserLessonValidator : AbstractValidator<GetByModuleByUserLessonRequest>
{
    public GetByModuleByUserLessonValidator()
    {
        // RuleFor(m => m.ModuleId)
        //    .NotEmpty();
        // RuleFor(m => m.UserId)
        //    .NotEmpty();
    }
}
