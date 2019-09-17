FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["OcelotServer.csproj", ""]
RUN dotnet restore "OcelotServer.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "OcelotServer.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "OcelotServer.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "OcelotServer.dll"]