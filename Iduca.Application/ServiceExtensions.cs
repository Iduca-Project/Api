using System.Reflection;
using Iduca.Application.Common.Exceptions;
using Iduca.Application.Common.Services;
using Iduca.Application.Contracts;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Iduca.Application.Common.Behaviors;

namespace Iduca.Application;

public static class ServiceExtensions
{
    public static void ConfigureApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        
        // Registrar serviços de autenticação
        services.AddScoped<IAuthenticator, JwtAuthenticator>();
        
        // Registrar serviços de auditoria
        services.AddScoped<ILogService, LogService>();
        
        // Registrar serviço de hierarquia
        services.AddScoped<IHierarchyService, HierarchyService>();
        
        // Registrar serviço de seed
        services.AddScoped<ISeedService, SeedService>();
    }
}