using FluentValidation;

namespace Iduca.Application.Features.Modules.Update;

public class UpdateModuleValidator : AbstractValidator<UpdateModuleRequest>
{
    public UpdateModuleValidator()
    {
        RuleFor(m => m.Id)
            .NotEmpty()
            .WithMessage("Id é obrigatório");

        RuleFor(m => m.Name)
            .NotEmpty()
            .WithMessage("Nome é obrigatório")
            .MinimumLength(2)
            .WithMessage("Nome deve ter pelo menos 2 caracteres")
            .MaximumLength(100)
            .WithMessage("Nome deve ter no máximo 100 caracteres");

        RuleFor(m => m.Description)
            .NotEmpty()
            .WithMessage("Descrição é obrigatória")
            .MinimumLength(10)
            .WithMessage("Descrição deve ter pelo menos 10 caracteres")
            .MaximumLength(500)
            .WithMessage("Descrição deve ter no máximo 500 caracteres");

        RuleFor(m => m.Index)
            .GreaterThan(0)
            .WithMessage("Índice deve ser maior que 0");

        RuleFor(m => m.CourseId)
            .NotEmpty()
            .WithMessage("Curso é obrigatório");
    }
}
