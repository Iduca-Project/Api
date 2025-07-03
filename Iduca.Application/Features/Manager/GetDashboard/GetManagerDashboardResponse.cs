namespace Iduca.Application.Features.Manager.GetDashboard;

public sealed record GetManagerDashboardResponse(
    int TotalEmployees,
    int TotalCourses,
    int TotalEnrollments,
    double CompletionRate,
    List<CategoryPerformance> PerformanceByCategory,
    CourseStatusInfo CourseStatus,
    HierarchyInfo HierarchyInfo,
    List<SubordinateInfo> SubordinatesList
);

public sealed record CategoryPerformance(
    string Category,
    double Score
);

public sealed record CourseStatusInfo(
    int Completed,
    int InProgress,
    int NotStarted
);

public sealed record HierarchyInfo(
    int ManagerLevel,
    int DirectSubordinates,
    int TotalSubordinates
);

public sealed record SubordinateInfo(
    Guid Id,
    string Name,
    string Email,
    bool IsDirect
);
