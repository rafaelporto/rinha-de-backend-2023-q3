namespace Rinha.Backend.Api.Data;

public interface IWriterContentStorage
{
    ValueTask Insert(string key, string searchCriteria, byte[] content, CancellationToken requestToken);    
}

internal sealed partial class ContentStorage: IWriterContentStorage
{
    public ValueTask Insert(string key, string searchCriteria, byte[] content, CancellationToken requestToken)
    {

        return ValueTask.CompletedTask;

    }
}
