
using FluentValidation;

namespace Iduca.Application.Features.Reminders.Update;

public class UpdateReminderValidator : AbstractValidator<UpdateReminderRequest>
{
    public UpdateReminderValidator()
    {
        //RuleFor(m => m.[propertie])
        //    .NotEmpty()
        //    .MaximumLength(64)
        //    .MinimumLength(8);
    }
}
