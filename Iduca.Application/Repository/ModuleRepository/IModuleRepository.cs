using Iduca.Domain.Models;

namespace Iduca.Application.Repository.ModuleRepository;

public interface IModuleRepository : IBaseRepository<Module>
{
    Task<Module?> GetModuleByEqualName(string name, CancellationToken cancellationToken);
    Task<List<Module>> GetModuleByCourseId(Guid id, CancellationToken cancellationToken);
}