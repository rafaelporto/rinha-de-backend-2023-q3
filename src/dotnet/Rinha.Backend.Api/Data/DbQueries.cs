namespace Rinha.Backend.Api.Data;

public static class DbQueries
{
    public const string InsertEntries = """
        INSERT INTO pessoas (id, apelido, search_criteria, content)
        VALUES (@id, @apelido, @search_criteria, @content)
        ON CONFLICT (id) DO UPDATE SET search_criteria = @search_criteria, content = @content
    """;

    public const string ReadSingleEntry = """
        SELECT content FROM pessoas WHERE id = @id
    """;

    public const string SearchEntry = """
        SELECT content FROM pessoas WHERE search_criteria LIKE @search_criteria
        LIMIT 50
    """;

    public const string CountEntries = """
        SELECT COUNT(pessoas.id) FROM pessoas
    """;
}
