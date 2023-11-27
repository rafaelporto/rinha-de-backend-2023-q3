namespace Rinha.Backend.Api;

public record Pessoa
{
    public string? Id { get; private set; }
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
    
    public Pessoa SetId()
    {
        Id = Guid.NewGuid().ToString();
        return this;
    } 
}
