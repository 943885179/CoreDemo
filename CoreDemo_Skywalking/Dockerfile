FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["CoreDemo_Skywalking.csproj", ""]
RUN dotnet restore "CoreDemo_Skywalking.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "CoreDemo_Skywalking.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "CoreDemo_Skywalking.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "CoreDemo_Skywalking.dll"]