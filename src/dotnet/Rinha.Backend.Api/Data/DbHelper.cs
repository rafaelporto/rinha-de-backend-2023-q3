namespace Rinha.Backend.Api.Data;

internal static class DbHelper
{
    public static string EnsureDbConnection()
    {
        var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("DB_CONNECTION_STRING não foi definida");
        }

        return connectionString;
    }
}
