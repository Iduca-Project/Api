using Iduca.Domain.Models;

namespace Iduca.Application.Repository.CompanyRepository;

public interface ICompanyRepository : IBaseRepository<Company>
{
    Task<List<Company>> GetCompanyByName(string name, CancellationToken cancellationToken);
    public Task<Company?> GetCompanyByEqualName(string name, CancellationToken cancellationToken);
}