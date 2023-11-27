namespace Rinha.Backend.Api.Data;

internal class Store : IStore
{
    public ValueTask<byte[]?> Get(string id, CancellationToken requestToken)
    {
        throw new NotImplementedException();
    }

    public ValueTask<IAsyncEnumerable<Pessoa>> Search(string searchTerm, CancellationToken requestToken)
    {
        throw new NotImplementedException();
    }

    public ValueTask Insert(Pessoa novaPessoa)
    {
        throw new NotImplementedException();
    }
}
