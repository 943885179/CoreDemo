FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["../MapApi1/MapApi1.csproj", "../MapApi1/"]
RUN dotnet restore "../MapApi1/MapApi1.csproj"
COPY . .
WORKDIR "/src/../MapApi1"
RUN dotnet build "MapApi1.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "MapApi1.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MapApi1.dll"]