using Iduca.Domain.Models;

namespace Iduca.Application.Features.Users.GetMyCourses;

public sealed record GetMyCoursesResponse(
    List<MyCourseInfo> Courses
);

public sealed record MyCourseInfo(
    Guid CourseId,
    string CourseName,
    string Description,
    string Image,
    DateTime EnrolledAt,
    CourseProgress Progress,
    string EstimatedTimeToComplete,
    string? Certificate
);

public sealed record CourseProgress(
    int TotalModules,
    int CompletedModules,
    int TotalLessons,
    int CompletedLessons,
    int TotalExercises,
    int CompletedExercises,
    double PercentageComplete,
    LastAccessedInfo? LastAccessedLesson
);

public sealed record LastAccessedInfo(
    Guid LessonId,
    string LessonTitle,
    string ModuleTitle
);
