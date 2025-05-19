using MediatR;

namespace Iduca.Application.Features.Companies.Delete;

public sealed record DeleteCompanyRequest(
    Guid Id
) : IRequest<DeleteCompanyResponse>;