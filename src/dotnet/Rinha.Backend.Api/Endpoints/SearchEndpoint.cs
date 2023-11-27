using Rinha.Backend.Api.Data;

namespace Rinha.Backend.Api.Endpoints;

public static class SearchEndpoint
{
    public static RouteHandlerBuilder Search(this IEndpointRouteBuilder routeBuilder)
    {
        return routeBuilder.MapGet("/pessoas",
                async ([AsParameters] string t,
                    HttpContext context,
                    IStore store,
                    CancellationToken requestToken) =>
        {
            var maybePessoas = await store.Search(t, requestToken);

            context.Response.StatusCode = 200;
            context.Response.ContentType = "application/json"; 
            return maybePessoas;
        });
    }
}
