using AutoMapper;
using Iduca.Application.Common.Exceptions;
using Iduca.Application.Repository;
using Iduca.Application.Repository.CourseRepository;
using Iduca.Application.Repository.UserCourseRepository;
using Iduca.Domain.Models;
using MediatR;

namespace Iduca.Application.Features.Courses.GetDetails;

public class GetCourseDetailsHandler(
    ICourseRepository courseRepository,
    IUserCourseRepository userCourseRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : IRequestHandler<GetCourseDetailsRequest, GetCourseDetailsResponse>
{
    private readonly ICourseRepository courseRepository = courseRepository;
    private readonly IUserCourseRepository userCourseRepository = userCourseRepository;
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly IMapper mapper = mapper;

    public async Task<GetCourseDetailsResponse> Handle(GetCourseDetailsRequest request, CancellationToken cancellationToken)
    {
        // Buscar o curso com módulos e lições
        var course = await courseRepository.GetById(request.CourseId, cancellationToken)
            ?? throw new NotFoundException("Curso não encontrado.");

        // Buscar participantes do curso
        var userCourses = await userCourseRepository.GetAllByCourseId(course.Id, cancellationToken);
        var participants = userCourses.Count;

        // Calcular progresso do usuário (se fornecido)
        int userProgress = 0;
        if (request.UserId.HasValue)
        {
            var completedLessonsCount = await userCourseRepository.GetCompletedLessonsCount(request.UserId.Value, course.Id, cancellationToken);
            var totalLessons = course.Modules.Sum(m => m.Lessons.Count);
            
            if (totalLessons > 0)
            {
                userProgress = (int)Math.Round((double)completedLessonsCount / totalLessons * 100);
            }
        }

        // Mapear módulos
        var modules = new List<ModuleDetails>();
        foreach (var module in course.Modules.OrderBy(m => m.Index))
        {
            var content = new List<ContentDetails>();
            
            // Adicionar lições do módulo
            foreach (var lesson in module.Lessons.OrderBy(l => l.CreatedAt))
            {
                bool isCompleted = false;
                if (request.UserId.HasValue)
                {
                    isCompleted = lesson.CompletedBy.Any(u => u.Id == request.UserId.Value);
                }

                content.Add(new ContentDetails(
                    lesson.Id,
                    1, // Tipo lição
                    lesson.Title,
                    isCompleted
                ));
            }

            // Adicionar exercícios do módulo (se existirem)
            foreach (var exercise in module.Exercises.OrderBy(e => e.CreatedAt))
            {
                bool isCompleted = false;
                // TODO: Implementar verificação de exercícios completados quando a funcionalidade estiver disponível

                content.Add(new ContentDetails(
                    exercise.Id,
                    3, // Tipo exercício 
                    exercise.Title,
                    isCompleted
                ));
            }

            modules.Add(new ModuleDetails(
                module.Id,
                module.Name,
                module.Description,
                module.Index,
                content
            ));
        }

        // Determinar categoria principal
        var categoryName = course.Categories.FirstOrDefault()?.Name ?? "Geral";

        // Formatar duração
        var duration = $"{course.TotalHours}";

        return new GetCourseDetailsResponse(
            course.Id,
            course.Name,
            course.Image,
            course.Description,
            4.5, // Rating padrão - pode ser implementado futuramente
            participants,
            userProgress,
            course.Difficulty,
            duration,
            categoryName,
            modules.Any(m => m.Content.Any(c => c.Type == 4)), // Tem exame se houver conteúdo tipo 4
            modules
        );
    }
}
