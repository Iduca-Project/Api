using Iduca.Domain.Models;

namespace Iduca.Application.Repositories;
public interface IBaseRepository<TEntity> where TEntity : BaseModel
{
    void Create(TEntity entity, CancellationToken cancellationToken);
    void Update(TEntity entity, CancellationToken cancellationToken);
    void Delete(TEntity entity, CancellationToken cancellationToken);
    Task<bool> Exists(Guid id, CancellationToken cancellationToken);
    Task<TEntity?> Get(Guid id, CancellationToken cancellationToken);
    Task<List<TEntity>> GetAll(CancellationToken cancellationToken); 
}
