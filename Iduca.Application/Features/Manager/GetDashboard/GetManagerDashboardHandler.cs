using AutoMapper;
using Iduca.Application.Common.Services;
using Iduca.Application.Repository;
using Iduca.Application.Repository.CourseRepository;
using Iduca.Application.Repository.UserCourseRepository;
using Iduca.Application.Repository.CategoryRepository;
using MediatR;

namespace Iduca.Application.Features.Manager.GetDashboard;

public class GetManagerDashboardHandler(
    IHierarchyService hierarchyService,
    ICourseRepository courseRepository,
    IUserCourseRepository userCourseRepository,
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : IRequestHandler<GetManagerDashboardRequest, GetManagerDashboardResponse>
{
    private readonly IHierarchyService hierarchyService = hierarchyService;
    private readonly ICourseRepository courseRepository = courseRepository;
    private readonly IUserCourseRepository userCourseRepository = userCourseRepository;
    private readonly ICategoryRepository categoryRepository = categoryRepository;
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly IMapper mapper = mapper;

    public async Task<GetManagerDashboardResponse> Handle(GetManagerDashboardRequest request, CancellationToken cancellationToken)
    {
        // Obter subordinados diretos e indiretos
        var directSubordinates = await hierarchyService.GetSubordinatesAsync(request.ManagerId, includeIndirect: false);
        var allSubordinates = await hierarchyService.GetSubordinatesAsync(request.ManagerId, includeIndirect: true);

        // Obter nível hierárquico do manager
        var hierarchyLevel = await hierarchyService.GetHierarchyLevelAsync(request.ManagerId, cancellationToken);

        // Calcular estatísticas de cursos
        var totalEnrollments = 0;
        var completedCourses = 0;
        var inProgressCourses = 0;
        var notStartedCourses = 0;
        var categoryStats = new Dictionary<string, List<double>>();

        foreach (var subordinate in allSubordinates)
        {
            var userCourses = await userCourseRepository.GetAllByUserId(subordinate.Id, cancellationToken);
            totalEnrollments += userCourses.Count;

            foreach (var userCourse in userCourses)
            {
                // Calcular progresso do curso
                var completedLessonsCount = await userCourseRepository.GetCompletedLessonsCount(subordinate.Id, userCourse.CourseId, cancellationToken);
                var course = await courseRepository.GetById(userCourse.CourseId, cancellationToken);
                
                if (course != null)
                {
                    var totalLessons = course.Modules.Sum(m => m.Lessons.Count);
                    var progressPercentage = totalLessons > 0 ? (double)completedLessonsCount / totalLessons * 100 : 0;

                    if (progressPercentage >= 100)
                        completedCourses++;
                    else if (progressPercentage > 0)
                        inProgressCourses++;
                    else
                        notStartedCourses++;

                    // Agrupar por categoria para análise de performance
                    foreach (var category in course.Categories)
                    {
                        if (!categoryStats.ContainsKey(category.Name))
                            categoryStats[category.Name] = new List<double>();
                        
                        categoryStats[category.Name].Add(progressPercentage);
                    }
                }
            }
        }

        // Calcular performance por categoria
        var performanceByCategory = categoryStats.Select(kvp => new CategoryPerformance(
            kvp.Key,
            kvp.Value.Count > 0 ? Math.Round(kvp.Value.Average(), 1) : 0
        )).ToList();

        // Calcular taxa de conclusão geral
        var completionRate = totalEnrollments > 0 ? Math.Round((double)completedCourses / totalEnrollments * 100, 1) : 0;

        // Obter total de cursos únicos
        var allCourseIds = new HashSet<Guid>();
        foreach (var subordinate in allSubordinates)
        {
            var userCourses = await userCourseRepository.GetAllByUserId(subordinate.Id, cancellationToken);
            foreach (var uc in userCourses)
            {
                allCourseIds.Add(uc.CourseId);
            }
        }

        // Mapear subordinados
        var subordinatesList = allSubordinates.Select(s => new SubordinateInfo(
            s.Id,
            s.Name,
            s.Email,
            directSubordinates.Any(d => d.Id == s.Id)
        )).ToList();

        return new GetManagerDashboardResponse(
            allSubordinates.Count,
            allCourseIds.Count,
            totalEnrollments,
            completionRate,
            performanceByCategory,
            new CourseStatusInfo(completedCourses, inProgressCourses, notStartedCourses),
            new HierarchyInfo(hierarchyLevel, directSubordinates.Count, allSubordinates.Count),
            subordinatesList
        );
    }
}
