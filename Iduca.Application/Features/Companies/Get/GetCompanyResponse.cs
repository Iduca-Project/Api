namespace Iduca.Application.Features.Companies.Get;

public sealed record GetCompanyResponse(
    Guid Id,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    DateTime? DisabledAt,
    string Name
);