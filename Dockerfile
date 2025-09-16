FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src

COPY ["Directory.Packages.props", "./"]
COPY ["RoyalRentApi.sln", "./"]

COPY ["RoyalRent.Web/RoyalRent.Web.csproj", "RoyalRent.Web/"]
COPY ["RoyalRent.Presentation/RoyalRent.Presentation.csproj", "RoyalRent.Presentation/"]
COPY ["RoyalRent.Infrastructure/RoyalRent.Infrastructure.csproj", "RoyalRent.Infrastructure/"]
COPY ["RoyalRent.Application/RoyalRent.Application.csproj", "RoyalRent.Application/"]
COPY ["RoyalRent.Domain/RoyalRent.Domain.csproj", "RoyalRent.Domain/"]
COPY ["RoyalRent.UnitTests/RoyalRent.UnitTests.csproj", "RoyalRent.UnitTests/"]

# Here we clear the NuGet cache and restore the dependencies
RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet restore RoyalRentApi.sln --verbosity normal

COPY . .

RUN --mount=type=cache,target=/root/.nuget/packages \
    dotnet publish "RoyalRent.Web/RoyalRent.Web.csproj" \
    -c $BUILD_CONFIGURATION -o /app/publish \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
ARG APP_USER=app
ARG APP_UID=1000

RUN adduser -S -u ${APP_UID:-1000} -G ${APP_USER} -h /app ${APP_USER} || true

WORKDIR /app

ENV ASPNETCORE_URLS=http://+8080
ENV DOTNET_RUNNING_IN_CONTAINER=true
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

EXPOSE 8080

COPY --from=build --chown=${APP_USER}:${APP_USER} /app/publish .

USER ${APP_USER}

ENTRYPOINT ["dotnet", "RoyalRent.Web.dll"]
