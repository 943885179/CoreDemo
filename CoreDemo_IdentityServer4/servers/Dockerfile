FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["servers/servers.csproj", "servers/"]
RUN dotnet restore "servers/servers.csproj"
COPY . .
WORKDIR "/src/servers"
RUN dotnet build "servers.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "servers.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "servers.dll"]