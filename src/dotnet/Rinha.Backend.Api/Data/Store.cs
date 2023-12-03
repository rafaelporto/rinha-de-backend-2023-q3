using System.Data;
using System.Threading.Channels;
using Npgsql;

namespace Rinha.Backend.Api.Data;

internal class Store: IStore
{
    private readonly WriterContentStorage writerStorage;
    private readonly ReaderContentStorage readerStorage;
    private readonly ILogger<Store> _logger;
    private readonly NpgsqlConnection _dbConnection;
    private readonly Channel<DbEntry> dbEntryChannel;
    private readonly ChannelWriter<DbEntry> dbEntryWriter;

    public Store(NpgsqlConnection dbConnection, 
        IHostApplicationLifetime applicationLifetime,
        ILoggerFactory loggerFactory)
    {
        var applicationToken = applicationLifetime.ApplicationStopping;
        _dbConnection = dbConnection;
        dbEntryChannel = Channel.CreateBounded<DbEntry>(1000);
        dbEntryWriter = dbEntryChannel.Writer;
        writerStorage = new WriterContentStorage(
            dbConnection,
            dbEntryChannel.Reader,
            applicationToken,
            loggerFactory.CreateLogger<WriterContentStorage>());
        
        readerStorage = new ReaderContentStorage(dbConnection, loggerFactory.CreateLogger<ReaderContentStorage>());
        
        _logger = loggerFactory.CreateLogger<Store>();
    }

    private async Task EnsureConnectionOpen(CancellationToken requestToken)
    {
        while (_dbConnection.State != ConnectionState.Open)
        {
            try
            {
                await _dbConnection.OpenAsync(requestToken);
                _logger.LogInformation("[ContentStorage][EnsureConnectionOpen] => Connection to database opened");
            }
            catch (NpgsqlException)
            {
                _logger.LogWarning("[ContentStorage][EnsureConnectionOpen] => Connection to database failed, retrying in 1s");
                await Task.Delay(1_000);
            }
        }
    }
    
    public async ValueTask Insert(Pessoa novaPessoa, CancellationToken requestToken)
    {
        requestToken.ThrowIfCancellationRequested();
        await EnsureConnectionOpen(requestToken);

        var dbEntry = new DbEntry
        {
            Id = novaPessoa.Id!,
            Apelido = novaPessoa.Apelido!,
            SearchCriteria = novaPessoa.SearchCriteria(),
            Content = novaPessoa.ToByteArray()
        };

        await dbEntryWriter.WriteAsync(dbEntry, requestToken); 
    }

    public async ValueTask<byte[]?> Get(string id, CancellationToken requestToken)
    {
        requestToken.ThrowIfCancellationRequested();

        await EnsureConnectionOpen(requestToken);
        return await readerStorage.ReadEntry(id, requestToken);
    }

    public async IAsyncEnumerable<byte[]> Search(string searchTerm, CancellationToken requestToken)
    {
        await EnsureConnectionOpen(requestToken);
        
        await foreach (var content in readerStorage.Search(searchTerm, requestToken))
            yield return content;
    }
    
    public async ValueTask<uint> CountEntries(CancellationToken requestToken)
    {
        requestToken.ThrowIfCancellationRequested();
        
        await EnsureConnectionOpen(requestToken);
        
        using var command = new NpgsqlCommand(DbQueries.CountEntries, _dbConnection);
        return Convert.ToUInt32(await command.ExecuteScalarAsync(requestToken));
    }
}
