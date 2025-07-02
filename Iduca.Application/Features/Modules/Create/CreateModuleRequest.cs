using Iduca.Domain.Models;
using MediatR;

namespace Iduca.Application.Features.Modules.Create;

public sealed record CreateModuleRequest(
    string Name,
    string Description,
    Guid CourseId,
    int? Index = null
) : IRequest<CreateModuleResponse>;