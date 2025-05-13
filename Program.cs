using System;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using Amazon;

var builder = WebApplication.CreateBuilder(args);

try
{
    var conn = new MySqlConnection(
        "server=iducadb.cres2a24ikk2.us-east-1.rds.amazonaws.com;" +
        "user=admin;" +
        "database=IducaDB;" +
        "port=3306;" +
        "password=admin1234");

    {
        conn.Open();
        Console.WriteLine("Conectado com sucesso!");

        var cmd = new MySqlCommand("SHOW DATABASES;", conn);
        var reader = cmd.ExecuteReader();
        {
            while (reader.Read())
            {
                Console.WriteLine(reader[0]);
            }
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Erro na conexão ou execução: {ex.Message}");
}

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseHttpsRedirection();
app.Run();