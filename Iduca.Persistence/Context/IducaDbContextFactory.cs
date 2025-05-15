using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Iduca.Application.Config;

namespace Iduca.Persistence.Context;


public class IducaDbContextFactory : IDesignTimeDbContextFactory<IducaContext>
{
    public IducaDbContextFactory CreateDbContext(string[] args)
    {
        DotEnv.Load();

        var optionsBuilder = new DbContextOptionsBuilder<IducaContext>();
        // Adicionar nessa linha o comando para conexão com o banco MySql
        
        return new IducaContext(optionsBuilder.Options);
    }
}