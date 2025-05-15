using Iduca.Persistence.Tables;
using Microsoft.EntityFrameworkCore;

namespace Iduca.Persistence.Context;

public class IducaContext(DbContextOptions<IducaContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ! Fazer a configuração de todas as tables nessa área
    }
}