using FluentValidation;

namespace Iduca.Application.Features.User.Update;

public class UpdateUserValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserValidator()
    {
        RuleFor(u => u.Id)
            .NotEmpty()
            .WithMessage("Id é obrigatório");

        RuleFor(u => u.Name)
            .NotEmpty()
            .WithMessage("Nome é obrigatório")
            .MinimumLength(2)
            .WithMessage("Nome deve ter pelo menos 2 caracteres")
            .MaximumLength(100)
            .WithMessage("Nome deve ter no máximo 100 caracteres");

        RuleFor(u => u.Identity)
            .NotEmpty()
            .WithMessage("Identidade é obrigatória")
            .MinimumLength(3)
            .WithMessage("Identidade deve ter pelo menos 3 caracteres")
            .MaximumLength(20)
            .WithMessage("Identidade deve ter no máximo 20 caracteres");

        RuleFor(u => u.Email)
            .NotEmpty()
            .WithMessage("Email é obrigatório")
            .EmailAddress()
            .WithMessage("Email deve ter um formato válido");

        RuleFor(u => u.CompanyId)
            .NotEmpty()
            .WithMessage("Empresa é obrigatória");
    }
}
