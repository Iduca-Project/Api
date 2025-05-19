using MediatR;

namespace Iduca.Application.Features.Companies.GetById;

public sealed record GetByIdCompanyRequest(
    Guid Id
) : IRequest<GetByIdCompanyResponse>;
