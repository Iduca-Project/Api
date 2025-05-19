using FluentValidation;

namespace Iduca.Application.Features.Companies.Update;

public class UpdateCompanyValidator : AbstractValidator<UpdateCompanyRequest>
{
    public UpdateCompanyValidator()
    {
        RuleFor(c => c.Props.NewName)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(50);
    }
}