using FluentValidation;

namespace Iduca.Application.Features.Companies.GetById;

public class GetByIdCompanyValidator : AbstractValidator<GetByIdCompanyRequest>
{
    public GetByIdCompanyValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty();
    }
}