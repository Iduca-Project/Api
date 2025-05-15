using Microsoft.EntityFrameworkCore;
using Iduca.Persistence.Context;
using Iduca.Application.Repository;
using Iduca.Domain.Models;

public class BaseRepository<TModel>(IducaContext Context) : IBaseRepository<TModel>
    where TModel : BaseModel
{
    protected readonly IDucaContext context = Context;
    protected readonly DbSet<TModel> dbSet = IDucaContext.Set<TModel>();

    public void Create(TModel entity)
        => context.Add(entity);

    public void Update(TModel entity)
        => context.Update(entity);
    
    
}