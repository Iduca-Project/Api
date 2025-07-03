namespace Iduca.Application.Features.Analytics.CategoryStats;

public sealed record GetCategoryStatsResponse(
    Guid CategoryId,
    string CategoryName,
    CategoryAnalytics Analytics,
    List<CoursePerformance> TopCourses,
    List<CompanyUsage> CompanyBreakdown,
    DateTime GeneratedAt
);

public sealed record CategoryAnalytics(
    int TotalCourses,
    int TotalEnrollments,
    int CompletedEnrollments,
    double AverageCompletionRate,
    int TotalHoursContent,
    int UniqueUsers,
    double PopularityRank // percentual em relação a outras categorias
);

public sealed record CoursePerformance(
    Guid CourseId,
    string CourseName,
    int Enrollments,
    int Completions,
    double CompletionRate,
    double AverageRating,
    int TotalHours
);

public sealed record CompanyUsage(
    Guid CompanyId,
    string CompanyName,
    int Enrollments,
    int UniqueUsers,
    double CompletionRate
);
