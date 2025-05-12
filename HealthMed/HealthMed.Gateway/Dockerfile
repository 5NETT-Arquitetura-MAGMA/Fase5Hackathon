FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080


FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["HealthMed.Gateway/HealthMed.Gateway.csproj", "HealthMed.Gateway/"]
COPY ["HealthMed.Migrator/HealthMed.Migrator.csproj", "HealthMed.Migrator/"]
RUN dotnet restore "./HealthMed.Gateway/HealthMed.Gateway.csproj"
COPY . .
WORKDIR "/src/HealthMed.Gateway"
RUN dotnet build "./HealthMed.Gateway.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./HealthMed.Gateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HealthMed.Gateway.dll"]