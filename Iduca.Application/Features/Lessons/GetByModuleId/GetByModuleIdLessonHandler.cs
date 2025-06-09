
using AutoMapper;
using Iduca.Application.Repository;
using Iduca.Application.Repository.LessonRepository;
using MediatR;
using MediatR.Pipeline;

namespace Iduca.Application.Features.Lessons.GetByModuleId;

public class GetByModuleIdLesson(
    IUnitOfWork unitOfWork,
    ILessonRepository lessonRepository,
    IMapper mapper
) : IRequestHandler<GetByModuleIdLessonRequest, GetByModuleIdLessonResponse>
{
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly ILessonRepository lessonRepository = lessonRepository;
    private readonly IMapper mapper = mapper;

    public async Task<GetByModuleIdLessonResponse> Handle(GetByModuleIdLessonRequest request, CancellationToken cancellationToken)
    {
        var lessons = await lessonRepository.GetLessonByModuleId(request.ModuleId, cancellationToken); 
        return mapper.Map<GetByModuleIdLessonResponse>(lessons);
    }
}
