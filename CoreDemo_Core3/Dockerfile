FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["CoreDemo_Core3.csproj", ""]
RUN dotnet restore "./CoreDemo_Core3.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "CoreDemo_Core3.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CoreDemo_Core3.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CoreDemo_Core3.dll"]