﻿using Microsoft.AspNetCore.Mvc;
using Rinha.Backend.Api.Data;

namespace Rinha.Backend.Api.Endpoints;

public static class PostEndpoint
{
    public static RouteHandlerBuilder Create(this IEndpointRouteBuilder routeBuilder)
    {
        return routeBuilder.MapPost("/pessoas", 
            async ([FromBody]Pessoa novaPessoa,
                    HttpContext context,
                    IStore store,
                    CancellationToken requestToken) =>
        {
            if (novaPessoa.IsValid())
            {
                context.Response.StatusCode = 400;
                return Results.UnprocessableEntity();
            }

            await store.Insert(novaPessoa, requestToken);
            return Results.Created($"pessoas/{novaPessoa.Id}", null);
        })
        .WithName("CriarPessoa")
        .WithOpenApi();
    }
}
