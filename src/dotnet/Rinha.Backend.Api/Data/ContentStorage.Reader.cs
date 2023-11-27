namespace Rinha.Backend.Api.Data;

public interface IReaderContentStorage 
{
    ValueTask<byte[]> ReadEntry(string key, CancellationToken requestToken);    
    ValueTask<IAsyncEnumerable<byte[]>> Search(string searchCriteria, CancellationToken requestToken);
}

internal sealed partial class ContentStorage : IReaderContentStorage
{
    public ValueTask<byte[]> ReadEntry(string key, CancellationToken requestToken)
    {
        throw new NotImplementedException();
    }

    public ValueTask<IAsyncEnumerable<byte[]>> Search(string searchCriteria, CancellationToken requestToken)
    {
        throw new NotImplementedException();
    }
}
