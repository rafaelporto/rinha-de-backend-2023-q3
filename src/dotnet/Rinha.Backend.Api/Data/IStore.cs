namespace Rinha.Backend.Api.Data;

internal interface IStore
{
    ValueTask Insert(Pessoa novaPessoa, CancellationToken requestToken);
    ValueTask<byte[]?> Get(string id, CancellationToken requestToken);
    IAsyncEnumerable<byte[]> Search(string searchTerm, CancellationToken requestToken);
    ValueTask<uint> CountEntries(CancellationToken requestToken);
}
