using Iduca.Domain.Models;

namespace Iduca.Application.Features.Companies.GetAll;

public sealed record GetAllCompanyResponse(
    Guid Id,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    DateTime? DisabledAt,
    string Name
);