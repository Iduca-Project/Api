using FluentValidation;

namespace Iduca.Application.Features.Companies.Get;

public class GetCompanyValidator : AbstractValidator<GetCompanyRequest>
{
    public GetCompanyValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(50);
    }
}