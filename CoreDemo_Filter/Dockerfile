FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["CoreDemo_Filter.csproj", ""]
RUN dotnet restore "./CoreDemo_Filter.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "CoreDemo_Filter.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CoreDemo_Filter.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CoreDemo_Filter.dll"]