using Iduca.Persistence.Context;
using Iduca.Persistence;

using Iduca.Api.Extensions;
using Iduca.Api.Middlewares.ExceptionHandlers;

using Iduca.Application;
using Iduca.Application.Config;
using System.Text.Json.Serialization;
using Iduca.Application.Features.Companies.Get;
using Iduca.Application.Features.Courses.GetByQuery;
using Iduca.Application.Features.Companies.GetAll;
using Iduca.Application.Features.Categories.GetAll;

DotEnv.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigurePersistence();
builder.Services.ConfigureApplication();

builder.Services.ConfigureCorsPolicy();

builder.Services.AddAutoMapper(typeof(GetCoursesMapper));
builder.Services.AddAutoMapper(typeof(GetAllCategoryMapper));

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

app.UseCors();
app.MapControllers();
app.UseErrorHandler();
app.Run();