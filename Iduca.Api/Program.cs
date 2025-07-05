using Iduca.Persistence.Context;
using Iduca.Persistence;

using Iduca.Api.Extensions;
using Iduca.Api.Middlewares.ExceptionHandlers;
using Iduca.Api.Middlewares;
using Iduca.Api.Security;

using Iduca.Application;
using Iduca.Application.Config;
using Iduca.Application.Common.Services;
using Iduca.Application.Common.Session;
using System.Text.Json.Serialization;
using Iduca.Application.Features.Companies.Get;
using Iduca.Application.Features.Courses.GetByQuery;

DotEnv.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigurePersistence();
builder.Services.ConfigureApplication();

// Registrar serviços específicos da API
builder.Services.AddScoped<IRequestSession, RequestSession>();

builder.Services.ConfigureCorsPolicy();

// Configurar autenticação JWT customizada
builder.Services.AddAuthentication("Bearer")
.AddScheme<Microsoft.AspNetCore.Authentication.AuthenticationSchemeOptions, CustomJwtAuthenticationHandler>("Bearer", options => { });

builder.Services.AddAuthorization();

builder.Services.AddAutoMapper(typeof(GetCoursesMapper));

builder.Services.AddControllers().AddJsonOptions(op =>
{
    op.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    op.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

var serviceScope = app.Services.CreateScope();
var context = serviceScope.ServiceProvider.GetRequiredService<IducaContext>()
    ?? throw new InvalidOperationException("Failed to resolve context from service provider");

context.Database.EnsureCreated();

// Executar seed de dados iniciais
var seedService = serviceScope.ServiceProvider.GetRequiredService<ISeedService>();
await seedService.EnsureDefaultDataAsync();

serviceScope.Dispose();

app.UseCors();

// Configurar middlewares na ordem correta
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseErrorHandler();
app.Run();