
using MediatR;

namespace Iduca.Application.Features.Lessons.DeleteById;

public sealed record DeleteByIdLessonRequest(
    Guid LessonId
) : IRequest<DeleteByIdLessonResponse>;
