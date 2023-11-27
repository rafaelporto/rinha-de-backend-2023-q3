using Npgsql;

namespace Rinha.Backend.Api.Data;

internal sealed partial class ContentStorage
{
    private readonly NpgsqlConnection _dbConnection;

    public ContentStorage(NpgsqlConnection connection)
    {
        _dbConnection = connection;
    }
}
