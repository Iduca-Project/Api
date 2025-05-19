using Iduca.Domain.Models;

namespace Iduca.Application.Repository.CompanyRepository;

public interface ICompanyRepository : IBaseRepository<Company>
{
    Task<List<Company>> GetCompanyByName(string name, CancellationToken cancellationToken);
}