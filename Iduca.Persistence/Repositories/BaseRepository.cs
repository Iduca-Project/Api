using Microsoft.EntityFrameworkCore;
using Iduca.Persistence.Context;
using Iduca.Application.Repository;
using Iduca.Domain.Models;
using Iduca.Application.Repositories;

public class BaseRepository<TModel>(IducaContext Context) : IBaseRepository<TModel>
    where TModel : BaseModel
{
    protected readonly IducaContext context = Context;
    protected readonly DbSet<TModel> dbSet = Context.Set<TModel>();

    public void Create(TModel entity, CancellationToken cancellationToken)
        => context.Add(entity);

    public void Update(TModel entity, CancellationToken cancellationToken)
        => context.Update(entity);

    public void Delete(TModel entity, CancellationToken cancellationToken)
    {
        entity.DisabledAt = DateTime.UtcNow;
        context.Update(entity);
    }

    public Task<TModel?> Get(Guid id, CancellationToken cancellationToken)
        => context
            .Set<TModel>()
            .Where(entity => entity.DisabledAt == null)
            .FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);

    public Task<List<TModel>> GetAll(CancellationToken cancellationToken)
        => context
            .Set<TModel>()
            .Where(entity => entity.DisabledAt == null)
            .ToListAsync(cancellationToken);
    
    public Task<bool> Exists(Guid id, CancellationToken cancellationToken)
        => dbSet.AnyAsync(e => 
            EF.Property<Guid>(e, "Id") == id && 
            EF.Property<Guid?>(e, "DeletedAt") == null, 
            cancellationToken
        );
}