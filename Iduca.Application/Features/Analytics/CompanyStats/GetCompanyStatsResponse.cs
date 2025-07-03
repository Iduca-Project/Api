namespace Iduca.Application.Features.Analytics.CompanyStats;

public sealed record GetCompanyStatsResponse(
    Guid CompanyId,
    string CompanyName,
    CompanyStatistics Statistics,
    List<CourseStatistics> CourseBreakdown,
    List<CategoryStatistics> CategoryBreakdown,
    DateTime GeneratedAt
);

public sealed record CompanyStatistics(
    int TotalUsers,
    int TotalCourses,
    int TotalEnrollments,
    int CompletedCourses,
    double AverageCompletionRate,
    int TotalHoursLearned,
    int ActiveUsers // usuários que completaram ao menos uma lição nos últimos 30 dias
);

public sealed record CourseStatistics(
    Guid CourseId,
    string CourseName,
    int Enrollments,
    int Completions,
    double CompletionRate,
    double AverageProgress,
    int TotalHours
);

public sealed record CategoryStatistics(
    Guid CategoryId,
    string CategoryName,
    int CoursesCount,
    int Enrollments,
    double PopularityPercentage
);
