using AutoMapper;
using Iduca.Application.Common.Exceptions;
using Iduca.Application.Repository;
using Iduca.Application.Repository.CompanyRepository;
using Iduca.Application.Repository.UserRepository;
using Iduca.Application.Repository.UserCourseRepository;
using Iduca.Domain.Common.Messages;
using Iduca.Domain.Models;
using MediatR;

namespace Iduca.Application.Features.Analytics.CompanyStats;

public class GetCompanyStatsHandler(
    ICompanyRepository companyRepository,
    IUserRepository userRepository,
    IUserCourseRepository userCourseRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : IRequestHandler<GetCompanyStatsRequest, GetCompanyStatsResponse>
{
    private readonly ICompanyRepository companyRepository = companyRepository;
    private readonly IUserRepository userRepository = userRepository;
    private readonly IUserCourseRepository userCourseRepository = userCourseRepository;
    private readonly IUnitOfWork unitOfWork = unitOfWork;
    private readonly IMapper mapper = mapper;

    public async Task<GetCompanyStatsResponse> Handle(GetCompanyStatsRequest request, CancellationToken cancellationToken)
    {
        // Verificar se a empresa existe
        var company = await companyRepository.Get(request.CompanyId, cancellationToken)
            ?? throw new NotFoundException("Empresa não encontrada.");

        // Buscar todos os usuários da empresa
        var companyUsers = await userRepository.GetUsersByCompany(request.CompanyId, cancellationToken);
        var userIds = companyUsers.Select(u => u.Id).ToList();

        // Buscar todos os cursos matriculados pelos usuários da empresa
        var userCourses = await userCourseRepository.GetAllByCompanyId(request.CompanyId, cancellationToken);

        // Filtrar por data se especificado
        if (request.StartDate.HasValue)
        {
            userCourses = userCourses.Where(uc => uc.CreatedAt >= request.StartDate.Value).ToList();
        }
        if (request.EndDate.HasValue)
        {
            userCourses = userCourses.Where(uc => uc.CreatedAt <= request.EndDate.Value).ToList();
        }

        // Calcular estatísticas gerais
        var totalUsers = companyUsers.Count;
        var uniqueCourses = userCourses.Select(uc => uc.Course).DistinctBy(c => c.Id).ToList();
        var totalCourses = uniqueCourses.Count;
        var totalEnrollments = userCourses.Count;

        // Calcular cursos completados (100% de progresso)
        var completedCourses = 0;
        var totalProgressSum = 0.0;

        foreach (var userCourse in userCourses)
        {
            var course = userCourse.Course;
            var totalLessons = course.Modules.Sum(m => m.Lessons.Count);
            
            if (totalLessons > 0)
            {
                var completedLessons = await userCourseRepository.GetCompletedLessonsCount(userCourse.UserId, course.Id, cancellationToken);
                var progressPercentage = (double)completedLessons / totalLessons * 100;
                totalProgressSum += progressPercentage;
                
                if (progressPercentage >= 100.0)
                {
                    completedCourses++;
                }
            }
        }

        var averageCompletionRate = totalEnrollments > 0 ? totalProgressSum / totalEnrollments : 0.0;
        var totalHoursLearned = uniqueCourses.Sum(c => c.TotalHours);

        // Calcular usuários ativos (últimos 30 dias)
        var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);
        var activeUsers = 0; // TODO: Implementar lógica para usuários ativos baseado em logs

        var companyStatistics = new CompanyStatistics(
            totalUsers,
            totalCourses,
            totalEnrollments,
            completedCourses,
            averageCompletionRate,
            totalHoursLearned,
            activeUsers
        );

        // Calcular estatísticas por curso
        var courseBreakdown = new List<CourseStatistics>();
        foreach (var course in uniqueCourses)
        {
            var courseEnrollments = userCourses.Where(uc => uc.CourseId == course.Id).ToList();
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
            var averageProgress = courseEnrollments.Count > 0 ? courseProgressSum / courseEnrollments.Count : 0.0;

            courseBreakdown.Add(new CourseStatistics(
                course.Id,
                course.Name,
                courseEnrollments.Count,
                courseCompletions,
                courseCompletionRate,
                averageProgress,
                course.TotalHours
            ));
        }

        // Calcular estatísticas por categoria
        var categoryBreakdown = new List<CategoryStatistics>();
        var allCategories = uniqueCourses.SelectMany(c => c.Categories).DistinctBy(cat => cat.Id).ToList();

        foreach (var category in allCategories)
        {
            var categoryCoursesCount = uniqueCourses.Count(c => c.Categories.Any(cat => cat.Id == category.Id));
            var categoryEnrollments = userCourses.Count(uc => uc.Course.Categories.Any(cat => cat.Id == category.Id));
            var popularityPercentage = totalEnrollments > 0 ? (double)categoryEnrollments / totalEnrollments * 100 : 0.0;

            categoryBreakdown.Add(new CategoryStatistics(
                category.Id,
                category.Name,
                categoryCoursesCount,
                categoryEnrollments,
                popularityPercentage
            ));
        }

        return new GetCompanyStatsResponse(
            company.Id,
            company.Name,
            companyStatistics,
            courseBreakdown,
            categoryBreakdown,
            DateTime.UtcNow
        );
    }
}
