
using AutoMapper;
using Iduca.Domain.Models;

namespace Iduca.Application.Features.Reminders.Update;

public class UpdateReminderMapper : Profile
{
    public UpdateReminderMapper()
    {
        CreateMap<UpdateReminderRequest, Reminder>();
        CreateMap<Reminder, UpdateReminderResponse>();
    }
}
