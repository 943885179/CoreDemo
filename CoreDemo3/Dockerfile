FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["coredemo3.csproj", ""]
RUN dotnet restore "coredemo3.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "coredemo3.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "coredemo3.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "coredemo3.dll"]