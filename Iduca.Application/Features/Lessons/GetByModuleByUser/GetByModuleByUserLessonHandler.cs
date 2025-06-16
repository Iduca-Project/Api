
using AutoMapper;
using Iduca.Application.Repository;
using Iduca.Application.Repository.LessonRepository;
using MediatR;

namespace Iduca.Application.Features.Lessons.GetByModuleByUser;

public class GetByModuleByUserLesson(
    IUnitOfWork unitOfWork,
    ILessonRepository lessonRepository,
    IMapper mapper
) : IRequestHandler<GetByModuleByUserLessonRequest, GetByModuleByUserLessonResponse>
{
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly ILessonRepository lessonRepository = lessonRepository;
    private readonly IMapper mapper = mapper;

    public async Task<GetByModuleByUserLessonResponse> Handle(GetByModuleByUserLessonRequest request, CancellationToken cancellationToken)
    {
        Console.WriteLine(request.UserId);
        Console.WriteLine(request.ModuleId);
        var lessonsfounded = await lessonRepository.GetLessonByModuleIdAndUser(request.ModuleId, request.UserId, cancellationToken);
        return mapper.Map<GetByModuleByUserLessonResponse>(lessonsfounded);
    }
}
