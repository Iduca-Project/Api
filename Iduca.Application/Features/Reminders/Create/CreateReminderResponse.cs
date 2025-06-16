
namespace Iduca.Application.Features.Reminders.Create;

public sealed record CreateReminderResponse(
    string Title,
    string Description,
    string Date
);
