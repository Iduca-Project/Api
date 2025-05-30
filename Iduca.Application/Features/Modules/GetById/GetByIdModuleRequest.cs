
using MediatR;

namespace Iduca.Application.Features.Module_.GetById;

public sealed record GetByIdModuleRequest(
    Guid Id
) : IRequest<GetByIdModuleResponse>;
