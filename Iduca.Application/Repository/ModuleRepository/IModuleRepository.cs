using Iduca.Domain.Models;

namespace Iduca.Application.Repository.ModuleRepository;

public interface IModuleRepository : IBaseRepository<Module>
{
    Task<Module?> GetModuleByEqualNameInCourse(string name, Guid CourseId, CancellationToken cancellationToken);
    Task<List<Module>> GetModuleByCourseId(Guid id, CancellationToken cancellationToken);
    Task<int> GetLastModuleIndexInCourse(Guid courseId, CancellationToken cancellationToken);
}