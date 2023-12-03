using System.Text;
using Rinha.Backend.Api.Data;

namespace Rinha.Backend.Api.Endpoints;

public static class CountEndpoint
{
    public static RouteHandlerBuilder Counter(this IEndpointRouteBuilder routeBuilder)
    {
        return routeBuilder.MapGet("/contagem-pessoas",
                async (HttpContext context,
                    IStore store,
                    CancellationToken requestToken) =>
        {
            var count = await store.CountEntries(requestToken);

            context.Response.StatusCode = 200;
            context.Response.ContentType = "text/plain";

            return Results.Text(count.ToString(), null, Encoding.UTF8);
        });
    }
}
