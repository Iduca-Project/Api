using Microsoft.EntityFrameworkCore;
using Iduca.Persistence.Tables;
// using MySql.Data.MySqlClient.Interceptors;

namespace Iduca.Persistence.Context;

public class IducaContext(DbContextOptions<IducaContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.
    }
}