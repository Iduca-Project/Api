
using AutoMapper;
using Iduca.Application.Repository;
using Iduca.Application.Repository.ReminderRepository;
using Iduca.Domain.Models;
using MediatR;

namespace Iduca.Application.Features.Reminders.Create;

public class CreateReminder(
    IUnitOfWork unitOfWork,
    IReminderRepository reminderRepository,
    IMapper mapper
) : IRequestHandler<CreateReminderRequest, CreateReminderResponse>
{
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly IReminderRepository reminderRepository = reminderRepository;
    private readonly IMapper mapper = mapper;

    public async Task<CreateReminderResponse> Handle(CreateReminderRequest request, CancellationToken cancellationToken)
    {
        var reminder = mapper.Map<Reminder>(request);

        reminderRepository.Create(reminder);

        await unitOfWork.Save(cancellationToken);

        return mapper.Map<CreateReminderResponse>(reminder);
    }
}
