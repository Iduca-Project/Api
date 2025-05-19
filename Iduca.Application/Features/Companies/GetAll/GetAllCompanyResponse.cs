using Iduca.Domain.Models;

namespace Iduca.Application.Features.Companies.GetAll;

public sealed record GetAllCompanyResponse(
    List<Company> companies,
    string Name
);