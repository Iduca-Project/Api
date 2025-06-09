
using MediatR;

namespace Iduca.Application.Features.Lessons.GetByModuleId;

public sealed record GetByModuleIdLessonRequest(
    Guid ModuleId
) : IRequest<GetByModuleIdLessonResponse>;
