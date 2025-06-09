
using AutoMapper;
using Iduca.Application.Common.Exceptions;
using Iduca.Application.Repository;
using Iduca.Application.Repository.LessonRepository;
using Iduca.Domain.Common.Messages;
using Iduca.Domain.Models;
using MediatR;

namespace Iduca.Application.Features.Lessons.Create;

public class CreateLesson(
    IUnitOfWork unitOfWork,
    ILessonRepository lessonRepository,
    IMapper mapper
) : IRequestHandler<CreateLessonRequest, CreateLessonResponse>
{
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly ILessonRepository lessonRepository = lessonRepository;
    private readonly IMapper mapper = mapper;

    public async Task<CreateLessonResponse> Handle(CreateLessonRequest request, CancellationToken cancellationToken)
    {
        var lesson = mapper.Map<Lesson>(request);

        var findLesson = await lessonRepository.GetLessonByEqualName(request.Title, cancellationToken);
        if (findLesson is not null)
            throw new DuplicityException(ExceptionMessage.DuplicityModel.LessonNameDuplicity);

        lessonRepository.Create(lesson);
        await unitOfWork.Save(cancellationToken);
        return mapper.Map<CreateLessonResponse>(lesson);
    }
}
