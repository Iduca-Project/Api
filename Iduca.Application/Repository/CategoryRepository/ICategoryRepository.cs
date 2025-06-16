using Iduca.Domain.Models;

namespace Iduca.Application.Repository.CategoryRepository;

public interface ICategoryRepository : IBaseRepository<Category>
{
    Task<Category?> GetCategoryByEqualName(string name, CancellationToken cancellationToken);
    Task<List<Category>> GetCategoriesBySimilarName(string name, CancellationToken cancellationToken);
    Task<List<Category>> GetByQuery(string? name, int page, int maxItens, CancellationToken cancellationToken);
}