using AutoMapper;
using Iduca.Application.Common.Exceptions;
using Iduca.Application.Repository;
using Iduca.Application.Repository.LessonRepository;
using Iduca.Application.Repository.UserRepository;
using Iduca.Application.Repository.UserCourseRepository;
using Iduca.Domain.Models;
using MediatR;

namespace Iduca.Application.Features.Lessons.GetDetails;

public class GetLessonDetailsHandler(
    ILessonRepository lessonRepository,
    IUserRepository userRepository,
    IUserCourseRepository userCourseRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : IRequestHandler<GetLessonDetailsRequest, GetLessonDetailsResponse>
{
    private readonly ILessonRepository lessonRepository = lessonRepository;
    private readonly IUserRepository userRepository = userRepository;
    private readonly IUserCourseRepository userCourseRepository = userCourseRepository;
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly IMapper mapper = mapper;

    public async Task<GetLessonDetailsResponse> Handle(GetLessonDetailsRequest request, CancellationToken cancellationToken)
    {
        // Buscar a lição com módulo e curso
        var lesson = await lessonRepository.GetWithModuleAndCourse(request.LessonId, cancellationToken)
            ?? throw new NotFoundException("Lição não encontrada.");

        // Verificar se a lição foi completada pelo usuário (se fornecido)
        bool isCompleted = false;
        if (request.UserId.HasValue)
        {
            isCompleted = lesson.CompletedBy.Any(u => u.Id == request.UserId.Value);
        }

        // Buscar a próxima lição no módulo
        NextLessonInfo? nextLesson = null;
        var module = lesson.Module;
        var lessonsInModule = module.Lessons.OrderBy(l => l.CreatedAt).ToList();
        var currentIndex = lessonsInModule.FindIndex(l => l.Id == lesson.Id);
        
        if (currentIndex >= 0 && currentIndex < lessonsInModule.Count - 1)
        {
            var nextLessonInModule = lessonsInModule[currentIndex + 1];
            nextLesson = new NextLessonInfo(
                nextLessonInModule.Id,
                1, // Tipo padrão
                nextLessonInModule.Title
            );
        }
        else
        {
            // Se não há próxima lição no módulo atual, buscar no próximo módulo
            var course = module.Course;
            var modulesInCourse = course.Modules.OrderBy(m => m.Index).ToList();
            var currentModuleIndex = modulesInCourse.FindIndex(m => m.Id == module.Id);
            
            if (currentModuleIndex >= 0 && currentModuleIndex < modulesInCourse.Count - 1)
            {
                var nextModule = modulesInCourse[currentModuleIndex + 1];
                var firstLessonInNextModule = nextModule.Lessons.OrderBy(l => l.CreatedAt).FirstOrDefault();
                
                if (firstLessonInNextModule != null)
                {
                    nextLesson = new NextLessonInfo(
                        firstLessonInNextModule.Id,
                        1, // Tipo padrão
                        firstLessonInNextModule.Title
                    );
                }
            }
        }

        var moduleInfo = new ModuleInfo(
            module.Id,
            module.Name,
            module.Description
        );

        return new GetLessonDetailsResponse(
            lesson.Id,
            1, // Tipo padrão (pode ser configurado futuramente)
            lesson.Title,
            lesson.Description,
            lesson.Module.CourseId,
            isCompleted,
            lesson.Contents,
            lesson.VideoLink,
            nextLesson,
            moduleInfo
        );
    }
}
