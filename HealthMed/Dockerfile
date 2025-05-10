FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080


FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["HealthMed.QueryAPI/HealthMed.QueryAPI.csproj", "HealthMed.QueryAPI/"]
RUN dotnet restore "./HealthMed.QueryAPI/HealthMed.QueryAPI.csproj"
COPY . .
WORKDIR "/src/HealthMed.QueryAPI"
RUN dotnet build "./HealthMed.QueryAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./HealthMed.QueryAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HealthMed.QueryAPI.dll"]