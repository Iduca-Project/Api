
using MediatR;

namespace Iduca.Application.Features.Module_.DeleteById;

public sealed record DeleteByIdModuleRequest(
    Guid Id
) : IRequest<DeleteByIdModuleResponse>;
