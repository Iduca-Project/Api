using AutoMapper;
using Iduca.Application.Common.Exceptions;
using Iduca.Application.Repository;
using Iduca.Application.Repository.LessonRepository;
using Iduca.Application.Repository.UserRepository;
using Iduca.Application.Repository.UserCourseRepository;
using Iduca.Domain.Common.Messages;
using Iduca.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iduca.Application.Features.Lessons.Complete;

public class CompleteLessonHandler(
    ILessonRepository lessonRepository,
    IUserRepository userRepository,
    IUserCourseRepository userCourseRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : IRequestHandler<CompleteLessonRequest, CompleteLessonResponse>
{
    private readonly ILessonRepository lessonRepository = lessonRepository;
    private readonly IUserRepository userRepository = userRepository;
    private readonly IUserCourseRepository userCourseRepository = userCourseRepository;
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly IMapper mapper = mapper;

    public async Task<CompleteLessonResponse> Handle(CompleteLessonRequest request, CancellationToken cancellationToken)
    {
        // Verificar se a lição existe
        var lesson = await lessonRepository.Get(request.LessonId, cancellationToken)
            ?? throw new NotFoundException("Lição não encontrada.");

        // Verificar se o usuário existe
        var user = await userRepository.Get(request.UserId, cancellationToken)
            ?? throw new NotFoundException("Usuário não encontrado.");

        // Verificar se o usuário está matriculado no curso desta lição
        var userCourse = await userCourseRepository.GetUserCourseByIds(request.UserId, lesson.Course.Id, cancellationToken)
            ?? throw new NotFoundException("Usuário não está matriculado neste curso.");

        // Verificar se a lição já foi completada
        if (lesson.CompletedBy.Any(u => u.Id == request.UserId))
        {
            return new CompleteLessonResponse(
                request.LessonId,
                request.UserId,
                DateTime.UtcNow,
                "Lição já foi concluída anteriormente.",
                0.0 // TODO: Calcular progresso atual
            );
        }

        // Marcar lição como concluída
        lesson.CompletedBy.Add(user);
        lessonRepository.Update(lesson);

        await unitOfWork.Save(cancellationToken);

        // Calcular novo progresso do curso
        var courseId = lesson.Module.CourseId;
        var completedLessonsCount = await userCourseRepository.GetCompletedLessonsCount(request.UserId, courseId, cancellationToken);
        
        // Buscar o curso para calcular o total de lições
        var course = lesson.Module.Course;
        var totalLessons = course.Modules.Sum(m => m.Lessons.Count);
        var progressPercentage = totalLessons > 0 
            ? (double)completedLessonsCount / totalLessons * 100 
            : 0.0;

        return new CompleteLessonResponse(
            request.LessonId,
            request.UserId,
            DateTime.UtcNow,
            "Lição concluída com sucesso!",
            progressPercentage
        );
    }
}
