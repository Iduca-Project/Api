
using Api.Core.Repositories;
using Iduca.Application.Config;
using Iduca.Application.Repository;
using Iduca.Persistence.Context;
using Iduca.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IAgro.Persistence;

public static class ServiceExtensions
{
    public static void ConfigurePersistence(this IServiceCollection services)
    {
        var connection = DotEnv.Get("DATABASE_URL");

        services.AddDbContext<IducaContext>(opt => opt.UseMySQL(connection));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped</* IUserRepository */, UserRepository>();
        services.AddScoped</* ICompanyRepository */, CompanyRepository>();
    }
}