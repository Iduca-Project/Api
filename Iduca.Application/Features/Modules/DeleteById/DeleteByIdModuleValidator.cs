
using FluentValidation;

namespace Iduca.Application.Features.Modules.DeleteById;

public class DeleteByIdModuleValidator : AbstractValidator<DeleteByIdModuleRequest>
{
    public DeleteByIdModuleValidator()
    {
        RuleFor(m => m.Id)
           .NotEmpty();
    }
}
