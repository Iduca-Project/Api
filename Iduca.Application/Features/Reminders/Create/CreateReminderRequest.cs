
using MediatR;

namespace Iduca.Application.Features.Reminders.Create;

public sealed record CreateReminderRequest(
    string Title,
    string Description,
    string Date,
    Guid UserId
) : IRequest<CreateReminderResponse>;
