using Iduca.Domain.Models;

namespace Iduca.Application.Repository.CompanyRepository;

public interface ICompanyRepository : IBaseRepository<Company>
{
    Task<Company?> GetCompanyByName(string name, CancellationToken cancellationToken);
}