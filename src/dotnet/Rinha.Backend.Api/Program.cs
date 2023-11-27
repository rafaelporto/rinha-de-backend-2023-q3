using Microsoft.Extensions.Logging.Abstractions;
using Rinha.Backend.Api;
using Rinha.Backend.Api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddNpgsqlDataSource(
    Environment.GetEnvironmentVariable(
        "DB_CONNECTION_STRING") ??
        "ERRO de connection string!!!", dataSourceBuilderAction: a => { a.UseLoggerFactory(NullLoggerFactory.Instance); });
builder.Services.AddSingleton<IStore, Store>();

var app = builder.Build();

app.MapEndpoints();

app.Run();
