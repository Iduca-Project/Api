using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Iduca.Application.Config;

namespace Iduca.Persistence.Context;


public class IducaDbContextFactory : IDesignTimeDbContextFactory<IducaContext>
{
    public IducaContext CreateDbContext(string[] args)
    {
        DotEnv.Load();

        var optionsBuilder = new DbContextOptionsBuilder<IducaContext>();

        optionsBuilder.UseMySql(
            DotEnv.Get("DATABASE_URL"),
            ServerVersion.AutoDetect(DotEnv.Get("DATABASE_URL"))
        );

        return new IducaContext(optionsBuilder.Options);
    }
}