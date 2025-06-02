using FluentValidation;

namespace Iduca.Application.Features.Categories.GetByName;

public class GetByNameCategoryValidator : AbstractValidator<GetByNameCategoryRequest>
{
    public GetByNameCategoryValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .MinimumLength(1)
            .MaximumLength(50);
    }
}