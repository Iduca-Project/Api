
using FluentValidation;

namespace Iduca.Application.Features.Courses.GetCourseOfUser;

public class GetCourseOfUserCourseValidator : AbstractValidator<GetCourseOfUserCourseRequest>
{
    public GetCourseOfUserCourseValidator()
    {
        //RuleFor(m => m.[propertie])
        //    .NotEmpty()
        //    .MaximumLength(64)
        //    .MinimumLength(8);
    }
}
