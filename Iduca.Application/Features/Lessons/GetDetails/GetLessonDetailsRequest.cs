using MediatR;

namespace Iduca.Application.Features.Lessons.GetDetails;

public sealed record GetLessonDetailsRequest(
    Guid LessonId,
    Guid? UserId = null
) : IRequest<GetLessonDetailsResponse>;
