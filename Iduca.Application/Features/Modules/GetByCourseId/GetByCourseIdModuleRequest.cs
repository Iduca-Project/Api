using Iduca.Domain.Models;
using MediatR;

namespace Iduca.Application.Features.Modules.Create;

public sealed record GetByCourseIdModuleRequest(
    Guid CourseId
) : IRequest<CreateModuleResponse>;