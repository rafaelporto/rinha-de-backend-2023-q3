using Npgsql;

namespace Rinha.Backend.Api.Data;

internal sealed class ReaderContentStorage(NpgsqlConnection connection, ILogger<ReaderContentStorage> logger)
{
    private readonly NpgsqlConnection _dbConnection = connection;
    private readonly ILogger<ReaderContentStorage> _logger = logger;

    public ValueTask<byte[]?> ReadEntry(string id, CancellationToken requestToken)
    {
        requestToken.ThrowIfCancellationRequested();

        try
        {
            using var command = new NpgsqlCommand(DbQueries.ReadSingleEntry, _dbConnection);
            command.Parameters.AddWithValue("id", id);
            return new ValueTask<byte[]?>(command.ExecuteScalar() as byte[]);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[ContentStorage][ReadEntry] => Failed to read entry from database");
            return default;
        }
    }

    public async IAsyncEnumerable<byte[]> Search(string searchCriteria, CancellationToken requestToken)
    {
        requestToken.ThrowIfCancellationRequested();

        using var command = new NpgsqlCommand(DbQueries.SearchEntry, _dbConnection);
        command.Parameters.AddWithValue("search_criteria", $"%{searchCriteria}%");
        var batchOfContents = new List<byte[]>();

        using var reader = await command.ExecuteReaderAsync(requestToken);
       
        while (await reader.ReadAsync(requestToken))
            yield return reader[0] as byte[];
    }
}
