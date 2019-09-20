FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["QuickstartIdentityServer/QuickstartIdentityServer.csproj", "QuickstartIdentityServer/"]
RUN dotnet restore "QuickstartIdentityServer/QuickstartIdentityServer.csproj"
COPY . .
WORKDIR "/src/QuickstartIdentityServer"
RUN dotnet build "QuickstartIdentityServer.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "QuickstartIdentityServer.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "QuickstartIdentityServer.dll"]