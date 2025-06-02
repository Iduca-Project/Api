
using AutoMapper;
using Iduca.Application.Repository;
using Iduca.Application.Repository.LessonRepository;
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
        


        await unitOfWork.Save(cancellationToken);
        return mapper.Map<CreateLessonResponse>(lesson);
    }
}
