
using FluentValidation;

namespace Iduca.Application.Features.Module_.DeleteById;

public class DeleteByIdModuleValidator : AbstractValidator<DeleteByIdModuleRequest>
{
    public DeleteByIdModuleValidator()
    {
        RuleFor(m => m.Id)
           .NotEmpty();
    }
}
