using MediatR;

namespace Iduca.Application.Features.Companies.Update;

public sealed record UpdateCompanyRequest(
    Guid Id,
    UpdateCompanyRequestProps Props
) : IRequest<UpdateCompanyResponse>;

public sealed record UpdateCompanyRequestProps (
    string NewName
);