
namespace Iduca.Application.Features.Lessons.Create;

public sealed record CreateLessonResponse(
    string Name,
    string Description,
    string? VideoUrl,
    Guid ModuleId
);
