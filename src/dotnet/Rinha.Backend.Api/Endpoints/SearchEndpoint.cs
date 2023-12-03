using System.Buffers;
using Microsoft.AspNetCore.Mvc;
using Rinha.Backend.Api.Data;

namespace Rinha.Backend.Api.Endpoints;

public static class SearchEndpoint
{
    public static RouteHandlerBuilder Search(this IEndpointRouteBuilder routeBuilder)
    {
        return routeBuilder.MapGet("/pessoas",
                async ([FromQuery] string t,
                    HttpContext context,
                    IStore store,
                    CancellationToken requestToken) =>
        {
            context.Response.StatusCode = 200;
            context.Response.ContentType = "application/json";
            await context.Response.StartAsync(requestToken);
            context.Response.BodyWriter.Write("["u8);

            var firstElement = true;

            await foreach (var content in store.Search(t, requestToken))
            {
                if (firstElement)
                    firstElement = false;
                else
                    context.Response.BodyWriter.Write(","u8);

                await context.Response.BodyWriter.WriteAsync(content, requestToken);
            }

            context.Response.BodyWriter.Write("]"u8);
            await context.Response.CompleteAsync();
        });
    }
}
