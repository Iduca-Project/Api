namespace Iduca.Application.Features.Companies.Create;

public sealed record CreateCompanyResponse(
    Guid Id,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    DateTime? DisabledAt,
    string Name
);