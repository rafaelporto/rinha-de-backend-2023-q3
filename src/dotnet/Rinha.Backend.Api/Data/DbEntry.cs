namespace Rinha.Backend.Api.Data;

public readonly struct DbEntry
{
    public string Id { get; init; }
    public string Apelido { get; init; }
    public string SearchCriteria { get; init; }
    public byte[] Content { get; init; }
}
