
using FluentValidation;

namespace Iduca.Application.Features.Categories.GetAll;

public class GetAllCategoryValidator : AbstractValidator<GetAllCategoryRequest>
{
    public GetAllCategoryValidator()
    {
        //RuleFor(m => m.[propertie])
        //    .NotEmpty()
        //    .MaximumLength(64)
        //    .MinimumLength(8);
    }
}
