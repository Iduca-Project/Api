using AutoMapper;
using Iduca.Application.Common.Exceptions;
using Iduca.Application.Repository;
using Iduca.Application.Repository.UserRepository;
using Iduca.Application.Repository.UserCourseRepository;
using Iduca.Domain.Common.Messages;
using Iduca.Domain.Models;
using MediatR;

namespace Iduca.Application.Features.Users.GetMyCourses;

public class GetMyCoursesHandler(
    IUserRepository userRepository,
    IUserCourseRepository userCourseRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : IRequestHandler<GetMyCoursesRequest, GetMyCoursesResponse>
{
    private readonly IUserRepository userRepository = userRepository;
    private readonly IUserCourseRepository userCourseRepository = userCourseRepository;
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly IMapper mapper = mapper;

    public async Task<GetMyCoursesResponse> Handle(GetMyCoursesRequest request, CancellationToken cancellationToken)
    {
        // Verificar se o usuário existe
        var user = await userRepository.Get(request.UserId, cancellationToken)
            ?? throw new NotFoundException("Usuário não encontrado.");

        // Buscar cursos do usuário com todas as informações necessárias
        var userCourses = await userCourseRepository.GetAllByUserId(request.UserId, cancellationToken);

        var myCourses = new List<MyCourseInfo>();

        foreach (var userCourse in userCourses)
        {
            var course = userCourse.Course;
            
            // Calcular progresso
            var totalModules = course.Modules.Count;
            var totalLessons = course.Modules.Sum(m => m.Lessons.Count);
            var totalExercises = course.Modules.Sum(m => m.Exercises.Count);

            // Calcular lições completadas pelo usuário
            var completedLessonIds = await userCourseRepository.GetCompletedLessonIds(request.UserId, course.Id, cancellationToken);
            var completedLessons = completedLessonIds.Count;

            // Calcular módulos completados (um módulo é considerado completo quando todas as suas lições foram completadas)
            var completedModules = 0;
            foreach (var module in course.Modules)
            {
                var moduleLessonIds = module.Lessons.Select(l => l.Id).ToList();
                if (moduleLessonIds.All(id => completedLessonIds.Contains(id)) && moduleLessonIds.Count > 0)
                {
                    completedModules++;
                }
            }

            // Por enquanto, exercícios completados será 0 (pode ser implementado futuramente)
            var completedExercises = 0;

            var percentageComplete = totalLessons > 0 
                ? (double)completedLessons / totalLessons * 100 
                : 0.0;

            // Encontrar a última lição acessada (mais recente completada)
            LastAccessedInfo? lastAccessedLesson = null;
            if (completedLessons > 0)
            {
                // Para simplificar, vamos pegar a última lição completada pelo ID
                var lastLessonId = completedLessonIds.LastOrDefault();
                if (lastLessonId != Guid.Empty)
                {
                    var lastLesson = course.Modules
                        .SelectMany(m => m.Lessons)
                        .FirstOrDefault(l => l.Id == lastLessonId);
                    
                    if (lastLesson != null)
                    {
                        var module = course.Modules.First(m => m.Id == lastLesson.ModuleId);
                        lastAccessedLesson = new LastAccessedInfo(
                            lastLesson.Id,
                            lastLesson.Title,
                            module.Name
                        );
                    }
                }
            }

            // Calcular tempo estimado (baseado no total de horas do curso)
            var estimatedTime = $"PT{course.TotalHours}H";

            var progress = new CourseProgress(
                totalModules,
                completedModules,
                totalLessons,
                completedLessons,
                totalExercises,
                completedExercises,
                percentageComplete,
                lastAccessedLesson
            );

            var myCourse = new MyCourseInfo(
                course.Id,
                course.Name,
                course.Description,
                course.Image,
                userCourse.CreatedAt,
                progress,
                estimatedTime,
                userCourse.Certificate
            );

            myCourses.Add(myCourse);
        }

        return new GetMyCoursesResponse(myCourses);
    }
}
