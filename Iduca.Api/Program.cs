using Iduca.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Iduca.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigurePersistence();

var app = builder.Build();

using var serviceScope = app.Services.CreateScope();
var context = serviceScope.ServiceProvider.GetRequiredService<IducaContext>();

context.Database.Migrate();

const string sql = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'iducadb'";
var tableNames = new List<string>();

using (var command = context.Database.GetDbConnection().CreateCommand())
{
    command.CommandText = sql;
    command.CommandType = System.Data.CommandType.Text;

    context.Database.OpenConnection();

    using (var result = command.ExecuteReader())
    {
        while (result.Read())
        {
            tableNames.Add(result.GetString(0));
        }
    }
}

Console.WriteLine("Tabelas no banco MySQL:");
foreach (var table in tableNames)
{
    Console.WriteLine($"- {table}");
}

app.UseHttpsRedirection();
app.Run();