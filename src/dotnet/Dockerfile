FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5020

ENV ASPNETCORE_URLS=http://+:5020

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["Rinha.Backend.Api/Rinha.Backend.Api.csproj", "Rinha.Backend.Api/"]
RUN dotnet restore "Rinha.Backend.Api/Rinha.Backend.Api.csproj"
COPY . .
WORKDIR "/src/Rinha.Backend.Api"
RUN dotnet build "Rinha.Backend.Api.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "Rinha.Backend.Api.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Rinha.Backend.Api.dll"]
