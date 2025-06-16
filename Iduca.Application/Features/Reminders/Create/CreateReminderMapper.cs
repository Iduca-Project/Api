
using AutoMapper;
using Iduca.Domain.Models;

namespace Iduca.Application.Features.Reminders.Create;

public class CreateReminderMapper : Profile
{
    public CreateReminderMapper()
    {
        CreateMap<CreateReminderRequest, Reminder>();
        CreateMap<Reminder, CreateReminderResponse>();
    }
}
