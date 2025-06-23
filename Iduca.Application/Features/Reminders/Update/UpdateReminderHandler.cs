
using AutoMapper;
using Iduca.Application.Common.Exceptions;
using Iduca.Application.Repository;
using Iduca.Application.Repository.ReminderRepository;
using Iduca.Domain.Common.Messages;
using MediatR;

namespace Iduca.Application.Features.Reminders.Update;

public class UpdateReminder(
    IUnitOfWork unitOfWork,
    IReminderRepository reminderRepository,
    IMapper mapper
) : IRequestHandler<UpdateReminderRequest, UpdateReminderResponse>
{
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly IReminderRepository reminderRepository = reminderRepository;
    private readonly IMapper mapper = mapper;

    public async Task<UpdateReminderResponse> Handle(UpdateReminderRequest request, CancellationToken cancellationToken)
    {
        var reminder = await reminderRepository.Get(request.Id, cancellationToken)
            ?? throw new NotFoundException(ExceptionMessage.NotFound.Default);

        reminder.Title = request.NewTitle;
        reminder.Description = request.NewDescription;
        reminder.Date = request.NewDate;

        await unitOfWork.Save(cancellationToken);
        return mapper.Map<UpdateReminderResponse>(reminder);
    }
}
