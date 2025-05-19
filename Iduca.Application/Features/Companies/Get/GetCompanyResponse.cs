using Iduca.Domain.Models;

namespace Iduca.Application.Features.Companies.Get;

public sealed record GetCompanyResponse(
    List<Company> Companies
);