using FluentValidation;

namespace Iduca.Application.Features.Auth.Login;

public class LoginValidator : AbstractValidator<LoginRequest>
{
    public LoginValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email é obrigatório")
            .EmailAddress()
            .WithMessage("Email deve ter um formato válido");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Senha é obrigatória")
            .MinimumLength(3)
            .WithMessage("Senha deve ter pelo menos 3 caracteres");
    }
}
