
using FluentValidation;

namespace Iduca.Application.Features.LessonUsers.Create;

public class CreateLessonuserValidator : AbstractValidator<CreateLessonuserRequest>
{
    public CreateLessonuserValidator()
    {
        //RuleFor(m => m.[propertie])
        //    .NotEmpty()
        //    .MaximumLength(64)
        //    .MinimumLength(8);
    }
}
