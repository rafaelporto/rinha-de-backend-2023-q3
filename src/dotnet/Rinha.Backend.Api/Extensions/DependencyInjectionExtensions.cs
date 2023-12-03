using Microsoft.Extensions.Logging.Abstractions;
using Npgsql;
using Rinha.Backend.Api.Data;

namespace Rinha.Backend.Api.Extensions;

internal static class DependencyInjectionExtensions
{
    internal static IServiceCollection AddServices(this IServiceCollection services)
    {
        var connectionString = DbHelper.EnsureDbConnection();
        // TODO: Verificar o lifecycle da conexão com o banco
        services.AddNpgsqlDataSource(connectionString,
            dataSourceBuilderAction: a => { a.UseLoggerFactory(NullLoggerFactory.Instance); });

        services.AddSingleton<IStore, Store>();
        
        return services;
    }
}
