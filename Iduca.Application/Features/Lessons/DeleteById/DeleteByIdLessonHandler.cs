
using AutoMapper;
using Iduca.Application.Common.Exceptions;
using Iduca.Application.Repository;
using Iduca.Application.Repository.LessonRepository;
using MediatR;

namespace Iduca.Application.Features.Lessons.DeleteById;

public class DeleteByIdLesson(
    IUnitOfWork unitOfWork,
    ILessonRepository lessonRepository,
    IMapper mapper
) : IRequestHandler<DeleteByIdLessonRequest, DeleteByIdLessonResponse>
{
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly ILessonRepository lessonRepository = lessonRepository;
    private readonly IMapper mapper = mapper;

    public async Task<DeleteByIdLessonResponse> Handle(DeleteByIdLessonRequest request, CancellationToken cancellationToken)
    {
        var lesson = await lessonRepository.Get(request.LessonId, cancellationToken)
            ?? throw new NotFoundException();

        lessonRepository.Delete(lesson);
        await unitOfWork.Save(cancellationToken);
        return new DeleteByIdLessonResponse();
    }
}
