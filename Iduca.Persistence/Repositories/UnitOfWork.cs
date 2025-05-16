using System.ComponentModel;
using Iduca.Application.Repositories;
using Iduca.Persistence.Context;

namespace Iduca.Persistence.Repositories;

public class UnitOfWork(IducaContext ctx) : IUnitOfWork
{
    private readonly IducaContext context = ctx;

    public Task Save(CancellationToken cancellationToken)
    {
        return context.SaveChangesAsync(cancellationToken);
    }
}