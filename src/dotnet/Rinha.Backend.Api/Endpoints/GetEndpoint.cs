using Rinha.Backend.Api.Data;

namespace Rinha.Backend.Api.Endpoints;

public static class GetEndpoint
{
    public static RouteHandlerBuilder Get(this IEndpointRouteBuilder routeBuilder)
    {
        return routeBuilder.MapGet("/pessoas/{id}",
                async (string id,
                    HttpContext context,
                    IStore store,
                    CancellationToken requestToken) =>
        {
            var maybePessoa = await store.Get(id, requestToken);

            context.Response.StatusCode = maybePessoa is null ? 404 : 200;
            context.Response.ContentType = "application/json"; 

            await context.Response.StartAsync(requestToken);
            await context.Response.BodyWriter.WriteAsync(maybePessoa, requestToken);
            await context.Response.CompleteAsync();
        });
    }
}
