namespace Rinha.Backend.Api.Data;

internal interface IStore
{
    ValueTask Insert(Pessoa novaPessoa);
    ValueTask<byte[]?> Get(string id, CancellationToken requestToken);
    ValueTask<IAsyncEnumerable<Pessoa>> Search(string searchTerm, CancellationToken requestToken);
}
