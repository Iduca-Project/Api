using MediatR;

namespace Iduca.Application.Features.Modules.Create;

public sealed record CreateModuleRequest(
    string Name,
    string Description,
    Guid CourseId
) : IRequest<CreateModuleResponse>;