using Iduca.Domain.Models;

namespace Iduca.Application.Features.Lessons.GetDetails;

public sealed record GetLessonDetailsResponse(
    Guid Id,
    int Type,
    string Title,
    string Description,
    Guid CourseId,
    bool Completed,
    List<Content> Content,
    string? VideoLink,
    NextLessonInfo? NextLesson,
    ModuleInfo Module
);

public sealed record NextLessonInfo(
    Guid Id,
    int Type,
    string Title
);

public sealed record ModuleInfo(
    Guid Id,
    string Name,
    string Description
);
