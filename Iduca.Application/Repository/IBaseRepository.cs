using Iduca.Domain.Models;

namespace Iduca.Application.Repository
{
    public interface IBaseRepository<TEntity> where TEntity : BaseModel
    {
        void Create(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        Task<bool> Exists(Guid id);
        Task<TEntity?> Get(Guid id);
        Task<List<TEntity>> GetAll(); 
    }
}