namespace Iduca.Application.Features.Companies.GetById;

public sealed record GetByIdCompanyResponse(
    Guid Id,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    DateTime? DisabledAt,
    string Name
);