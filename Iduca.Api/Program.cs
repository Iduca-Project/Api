using Iduca.Persistence.Context;
using Iduca.Persistence;

using Iduca.Api.Extensions;

using Iduca.Application;
using Iduca.Application.Config;
using System.Text.Json.Serialization;

DotEnv.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigurePersistence();
builder.Services.ConfigureApplication();

builder.Services.ConfigureCorsPolicy();

builder.Services
    .AddControllers()
    .AddJsonOptions(options => options
        .JsonSerializerOptions
        .Converters
        .Add(new JsonStringEnumConverter())
    );

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

var serviceScope = app.Services.CreateScope();
var context = serviceScope.ServiceProvider.GetRequiredService<IducaContext>()
    ?? throw new InvalidOperationException("Failed to resolve context from service provider");

context.Database.EnsureCreated();

app.UseCors();
app.MapControllers();
app.Run();