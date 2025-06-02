
using MediatR;

namespace Iduca.Application.Features.Modules.GetById;

public sealed record GetByIdModuleRequest(
    Guid Id
) : IRequest<GetByIdModuleResponse>;
