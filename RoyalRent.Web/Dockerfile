FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["RoyalRent.Web/RoyalRent.Web.csproj", "RoyalRent.Web/"]
RUN dotnet restore "RoyalRent.Web/RoyalRent.Web.csproj"
COPY . .
WORKDIR "/src/RoyalRent.Web"
RUN dotnet build "RoyalRent.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "RoyalRent.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RoyalRent.Web.dll"]
