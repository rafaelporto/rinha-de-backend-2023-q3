using Npgsql;
using System.Data;
using System.Threading.Channels;

namespace Rinha.Backend.Api.Data;

internal sealed class WriterContentStorage
{
    private readonly NpgsqlConnection _dbConnection;
    private readonly ILogger<WriterContentStorage> _logger;
    private readonly ChannelReader<DbEntry> _reader;
    private readonly CancellationToken _applicationToken;
    private readonly Task consumerTask;
    private const int BATCH_SIZE = 1000;

    public WriterContentStorage(NpgsqlConnection connection,
        ChannelReader<DbEntry> incomingChannel,
        CancellationToken applicationToken,
        ILogger<WriterContentStorage> logger)
    {
        _applicationToken = applicationToken;
        _dbConnection = connection;
        _reader = incomingChannel;
        _logger = logger;
        consumerTask = Task.Factory.StartNew(
            async () => await StartConsumer(),
            default,
            TaskCreationOptions.HideScheduler,
            TaskScheduler.Default).Unwrap();
    }

    private async Task StartConsumer()
    {
        List<DbEntry> dbEntryPool = [];

        while (!_applicationToken.IsCancellationRequested)
        {
            await _reader.WaitToReadAsync(_applicationToken);

            while (dbEntryPool.Count < BATCH_SIZE && _reader.TryRead(out var entry))
                dbEntryPool.Add(entry);

            if (dbEntryPool is { Count: 0 })
                continue;

            try
            {
                var batchCommand = _dbConnection.CreateBatch();

                var command = new NpgsqlBatchCommand(DbQueries.InsertEntries)
                {
                    CommandType = CommandType.Text
                };

                foreach (var dbEntry in dbEntryPool)
                {
                    command.Parameters.AddWithValue("id", dbEntry.Id);
                    command.Parameters.AddWithValue("apelido", dbEntry.Apelido);
                    command.Parameters.AddWithValue("search_criteria", dbEntry.SearchCriteria);
                    command.Parameters.AddWithValue("content", dbEntry.Content);
                }

                batchCommand.BatchCommands.Add(command);

                await batchCommand.ExecuteNonQueryAsync(_applicationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[ContentStorage][StartConsumer] => Failed to insert entries into database");
            }
            finally
            {
                dbEntryPool.Clear();
            }
        }
    }
}
