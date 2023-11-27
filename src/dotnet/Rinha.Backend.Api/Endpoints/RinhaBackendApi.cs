using Rinha.Backend.Api.Endpoints;

namespace Rinha.Backend.Api;

public static class RinhaBackendApi
{
    public static void MapEndpoints(this IEndpointRouteBuilder app)
    {
        app.Create();
       // app.Get();
       // app.Search();
    }
}
