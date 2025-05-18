using MediatR;

namespace Iduca.Application.Features.Companies.Create;

public sealed record CreateCompanyRequest(
    string Name
) : IRequest<CreateCompanyResponse>;