
using MediatR;

namespace Iduca.Application.Features.Lessons.GetByModuleByUser;

public sealed record GetByModuleByUserLessonRequest(
    Guid ModuleId,
    Guid UserId
) : IRequest<GetByModuleByUserLessonResponse>;