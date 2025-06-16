
using Iduca.Domain.Models;

namespace Iduca.Application.Features.Lessons.GetByModuleByUser;

public sealed record GetByModuleByUserLessonResponse(
    List<UserLesson> LessonUser
);
public sealed record UserLesson(
    Guid LessonId,
    string Title,
    string Description,
    string VideoLink,
    List<Content> Contents,
    bool IsCompleted
);
