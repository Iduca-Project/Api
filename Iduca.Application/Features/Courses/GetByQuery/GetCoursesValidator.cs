using FluentValidation;

namespace Iduca.Application.Features.Courses.GetByQuery;

public class GetCoursesValidator : AbstractValidator<GetCoursesRequest>
{
    public GetCoursesValidator()
    {
    }
}