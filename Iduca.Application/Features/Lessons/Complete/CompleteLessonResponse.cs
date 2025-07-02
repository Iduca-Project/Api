namespace Iduca.Application.Features.Lessons.Complete;

public sealed record CompleteLessonResponse(
    Guid LessonId,
    Guid UserId,
    DateTime CompletedAt,
    string Message,
    double CourseProgressPercentage
);
