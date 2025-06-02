
using MediatR;

namespace Iduca.Application.Features.Modules.DeleteById;

public sealed record DeleteByIdModuleRequest(
    Guid Id
) : IRequest<DeleteByIdModuleResponse>;
