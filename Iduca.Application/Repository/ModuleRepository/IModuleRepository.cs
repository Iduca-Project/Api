using Iduca.Domain.Models;

namespace Iduca.Application.Repository.ModuleRepository;

public interface IModuleRepository : IBaseRepository<Module>
{
    Task<Module?> GetModuleByEqualName(string name, CancellationToken cancellationToken);
}