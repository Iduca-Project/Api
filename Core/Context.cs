using Microsoft.EntityFrameworkCore;

using Api.Core.Mapping;
using Api.Domain.Models;

namespace Api.Core;

public partial class Context : DbContext
{
    public Context() {}

    public Context(DbContextOptions<Context> options)
         : base(options)
    {}

     protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}