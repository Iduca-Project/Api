using MediatR;

namespace Iduca.Application.Features.Lessons.Complete;

public sealed record CompleteLessonRequest(
    Guid LessonId,
    Guid UserId
) : IRequest<CompleteLessonResponse>;
