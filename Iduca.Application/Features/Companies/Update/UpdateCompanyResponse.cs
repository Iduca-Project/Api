namespace Iduca.Application.Features.Companies.Update;

public sealed record UpdateCompanyResponse(
    Guid Id,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    DateTime? DisabledAt,
    string Name
);