using Rinha.Backend.Api;
using Rinha.Backend.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddServices();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(setup =>
{
    setup.SwaggerEndpoint("/swagger/v1/swagger.json", "Rinha Backend API");
    setup.RoutePrefix = string.Empty;
});

app.MapEndpoints();

app.Run();
