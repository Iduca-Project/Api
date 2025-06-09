
using MediatR;

namespace Iduca.Application.Features.Lessons.Create;

public sealed record CreateLessonRequest(
    string Title,
    string Description,
    string? VideoUrl,
    Guid ModuleId
) : IRequest<CreateLessonResponse>;
