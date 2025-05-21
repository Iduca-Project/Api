using FluentValidation;

namespace Iduca.Application.Features.Modules.Create;

public class CreateModuleValidator : AbstractValidator<CreateModuleRequest>
{
    public CreateModuleValidator()
    {
        RuleFor(m => m.Name)
            .NotEmpty()
            .MaximumLength(64)
            .MinimumLength(8);

        RuleFor(m => m.Description)
            .NotEmpty()
            .MaximumLength(511)
            .MinimumLength(8);
    }
}