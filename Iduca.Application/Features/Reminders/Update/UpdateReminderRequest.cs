
using MediatR;

namespace Iduca.Application.Features.Reminders.Update;

public sealed record UpdateReminderRequest(
    Guid Id,
    string NewTitle,
    string NewDescription,
    DateTime NewDate
) : IRequest<UpdateReminderResponse>;
