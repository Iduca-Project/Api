using AutoMapper;
using Iduca.Application.Common.Exceptions;
using Iduca.Application.Repository;
using Iduca.Application.Repository.CategoryRepository;
using Iduca.Application.Repository.CourseRepository;
using Iduca.Application.Repository.UserCourseRepository;
using Iduca.Domain.Common.Messages;
using Iduca.Domain.Models;
using MediatR;

namespace Iduca.Application.Features.Analytics.CategoryStats;

public class GetCategoryStatsHandler(
    ICategoryRepository categoryRepository,
    ICourseRepository courseRepository,
    IUserCourseRepository userCourseRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : IRequestHandler<GetCategoryStatsRequest, GetCategoryStatsResponse>
{
    private readonly ICategoryRepository categoryRepository = categoryRepository;
    private readonly ICourseRepository courseRepository = courseRepository;
    private readonly IUserCourseRepository userCourseRepository = userCourseRepository;
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly IMapper mapper = mapper;

    public async Task<GetCategoryStatsResponse> Handle(GetCategoryStatsRequest request, CancellationToken cancellationToken)
    {
        // Verificar se a categoria existe
        var category = await categoryRepository.Get(request.CategoryId, cancellationToken)
            ?? throw new NotFoundException("Categoria não encontrada.");

        // Buscar todos os cursos desta categoria
        var categoryCourses = await courseRepository.GetCoursesByCategory(request.CategoryId, cancellationToken);
        var courseIds = categoryCourses.Select(c => c.Id).ToList();

        // Buscar todas as matrículas nos cursos desta categoria
        var allUserCourses = new List<UserCourse>();
        foreach (var courseId in courseIds)
        {
            var courseEnrollments = await userCourseRepository.GetAllByCourseId(courseId, cancellationToken);
            allUserCourses.AddRange(courseEnrollments);
        }

        // Filtrar por data se especificado
        if (request.StartDate.HasValue)
        {
            allUserCourses = allUserCourses.Where(uc => uc.CreatedAt >= request.StartDate.Value).ToList();
        }
        if (request.EndDate.HasValue)
        {
            allUserCourses = allUserCourses.Where(uc => uc.CreatedAt <= request.EndDate.Value).ToList();
        }

        // Carregar dados completos dos cursos para cálculos
        var coursesWithData = new List<Course>();
        foreach (var courseId in courseIds)
        {
            var course = await courseRepository.GetWithModulesAndLessons(courseId, cancellationToken);
            if (course != null)
            {
                coursesWithData.Add(course);
            }
        }

        // Calcular estatísticas gerais da categoria
        var totalCourses = categoryCourses.Count;
        var totalEnrollments = allUserCourses.Count;
        var uniqueUsers = allUserCourses.Select(uc => uc.UserId).Distinct().Count();
        var totalHoursContent = coursesWithData.Sum(c => c.TotalHours);

        // Calcular matrículas completadas
        var completedEnrollments = 0;
        var totalProgressSum = 0.0;

        foreach (var userCourse in allUserCourses)
        {
            var course = coursesWithData.FirstOrDefault(c => c.Id == userCourse.CourseId);
            if (course != null)
            {
                var totalLessons = course.Modules.Sum(m => m.Lessons.Count);
                if (totalLessons > 0)
                {
                    var completedLessons = await userCourseRepository.GetCompletedLessonsCount(userCourse.UserId, course.Id, cancellationToken);
                    var progressPercentage = (double)completedLessons / totalLessons * 100;
                    totalProgressSum += progressPercentage;
                    
                    if (progressPercentage >= 100.0)
                    {
                        completedEnrollments++;
                    }
                }
            }
        }

        var averageCompletionRate = totalEnrollments > 0 ? totalProgressSum / totalEnrollments : 0.0;

        // Calcular rank de popularidade (por enquanto será 0, pode ser implementado comparando com outras categorias)
        var popularityRank = 0.0;

        var categoryAnalytics = new CategoryAnalytics(
            totalCourses,
            totalEnrollments,
            completedEnrollments,
            averageCompletionRate,
            totalHoursContent,
            uniqueUsers,
            popularityRank
        );

        // Calcular performance dos top cursos
        var topCourses = new List<CoursePerformance>();
        foreach (var course in coursesWithData.Take(10)) // Top 10 cursos
        {
            var courseEnrollments = allUserCourses.Where(uc => uc.CourseId == course.Id).ToList();
            var courseCompletions = 0;
            var courseProgressSum = 0.0;

            foreach (var enrollment in courseEnrollments)
            {
                var totalLessons = course.Modules.Sum(m => m.Lessons.Count);
                if (totalLessons > 0)
                {
                    var completedLessons = await userCourseRepository.GetCompletedLessonsCount(enrollment.UserId, course.Id, cancellationToken);
                    var progressPercentage = (double)completedLessons / totalLessons * 100;
                    courseProgressSum += progressPercentage;
                    
                    if (progressPercentage >= 100.0)
                    {
                        courseCompletions++;
                    }
                }
            }

            var courseCompletionRate = courseEnrollments.Count > 0 ? (double)courseCompletions / courseEnrollments.Count * 100 : 0.0;
            var averageRating = 0.0; // TODO: Implementar sistema de avaliação

            topCourses.Add(new CoursePerformance(
                course.Id,
                course.Name,
                courseEnrollments.Count,
                courseCompletions,
                courseCompletionRate,
                averageRating,
                course.TotalHours
            ));
        }

        // Ordenar por número de matrículas
        topCourses = topCourses.OrderByDescending(c => c.Enrollments).ToList();

        // Calcular breakdown por empresa
        var companyBreakdown = new List<CompanyUsage>();
        var usersByCompany = allUserCourses
            .GroupBy(uc => uc.User?.CompanyId)
            .Where(g => g.Key.HasValue)
            .ToList();

        foreach (var companyGroup in usersByCompany)
        {
            var companyEnrollments = companyGroup.ToList();
            var companyUniqueUsers = companyEnrollments.Select(uc => uc.UserId).Distinct().Count();
            
            // Calcular taxa de conclusão da empresa
            var companyCompletions = 0;
            foreach (var enrollment in companyEnrollments)
            {
                var course = coursesWithData.FirstOrDefault(c => c.Id == enrollment.CourseId);
                if (course != null)
                {
                    var totalLessons = course.Modules.Sum(m => m.Lessons.Count);
                    if (totalLessons > 0)
                    {
                        var completedLessons = await userCourseRepository.GetCompletedLessonsCount(enrollment.UserId, course.Id, cancellationToken);
                        var progressPercentage = (double)completedLessons / totalLessons * 100;
                        
                        if (progressPercentage >= 100.0)
                        {
                            companyCompletions++;
                        }
                    }
                }
            }

            var companyCompletionRate = companyEnrollments.Count > 0 ? (double)companyCompletions / companyEnrollments.Count * 100 : 0.0;

            // Buscar nome da empresa (assumindo que existe a relação)
            var companyName = companyEnrollments.FirstOrDefault()?.User?.Company?.Name ?? "Empresa não identificada";

            companyBreakdown.Add(new CompanyUsage(
                companyGroup.Key!.Value,
                companyName,
                companyEnrollments.Count,
                companyUniqueUsers,
                companyCompletionRate
            ));
        }

        return new GetCategoryStatsResponse(
            category.Id,
            category.Name,
            categoryAnalytics,
            topCourses,
            companyBreakdown,
            DateTime.UtcNow
        );
    }
}
