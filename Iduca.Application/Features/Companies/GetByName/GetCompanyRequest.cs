using MediatR;

namespace Iduca.Application.Features.Companies.Get;

public sealed record GetCompanyRequest(
    string Name
) : IRequest<GetCompanyResponse>;
