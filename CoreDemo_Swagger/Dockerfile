FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["CoreDemo_Swagger.csproj", ""]
RUN dotnet restore "CoreDemo_Swagger.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "CoreDemo_Swagger.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "CoreDemo_Swagger.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "CoreDemo_Swagger.dll"]