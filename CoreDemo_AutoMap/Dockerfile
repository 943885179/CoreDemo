FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["CoreDemo_AutoMap.csproj", ""]
RUN dotnet restore "CoreDemo_AutoMap.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "CoreDemo_AutoMap.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "CoreDemo_AutoMap.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "CoreDemo_AutoMap.dll"]