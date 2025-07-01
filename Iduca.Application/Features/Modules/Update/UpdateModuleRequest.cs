using MediatR;

namespace Iduca.Application.Features.Modules.Update;

public sealed record UpdateModuleRequest(
    Guid Id,
    string Name,
    string Description,
    int Index,
    Guid CourseId
) : IRequest<UpdateModuleResponse>;
