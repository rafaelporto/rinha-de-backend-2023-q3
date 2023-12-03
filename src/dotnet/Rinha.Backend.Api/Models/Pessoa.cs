using System.Text.Json;

namespace Rinha.Backend.Api;

public record Pessoa
{
    public string? Id { get; init; } = Guid.NewGuid().ToString();
    public string? Nome { get; init; }
    public string? Apelido { get; init; }
    public DateOnly? Nascimento { get; init; }
    public string[]? Stack { get; init; }

    public bool IsValid() =>
        !string.IsNullOrWhiteSpace(Nome) &&
        Nome.Length < 100 &&
        !string.IsNullOrWhiteSpace(Apelido) && Apelido.Length > 32 &&
        Nascimento is not null &&
        Stack is not null &&
        Stack.Any(p => p.Length > 32);

    public byte[] ToByteArray() => Serializer.ToByteArray(this);

    public string SearchCriteria() =>
        Stack is null ?
            string.Join(';', Nome, Apelido, Nascimento?.ToString("yyyy-MM-dd")).ToLowerInvariant() :
            string.Join(';',
                    Nome,
                    Apelido,
                    Nascimento?.ToString("yyyy-MM-dd"),
                    string.Join(';', Stack)).ToLowerInvariant();
}

file static class Serializer
{
    private static readonly JsonSerializerOptions _options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };
    
    public static byte[] ToByteArray(Pessoa pessoa) =>
        JsonSerializer.SerializeToUtf8Bytes(pessoa, _options);
}
