FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["JsClient/JsClient.csproj", "JsClient/"]
RUN dotnet restore "JsClient/JsClient.csproj"
COPY . .
WORKDIR "/src/JsClient"
RUN dotnet build "JsClient.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "JsClient.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "JsClient.dll"]