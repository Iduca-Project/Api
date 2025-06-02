
using FluentValidation;

namespace Iduca.Application.Features.Modules.GetById;

public class GetByIdModuleValidator : AbstractValidator<GetByIdModuleRequest>
{
    public GetByIdModuleValidator()
    {
        //RuleFor(m => m.[propertie])
        //    .NotEmpty()
        //    .MaximumLength(64)
        //    .MinimumLength(8);
    }
}
