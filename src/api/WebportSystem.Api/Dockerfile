# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Build.props", "."]
COPY ["src/api/WebportSystem.Api/WebportSystem.Api.csproj", "src/api/WebportSystem.Api/"]
COPY ["src/modules/identity/WebportSystem.Identity.Infrastructure/WebportSystem.Identity.Infrastructure.csproj", "src/modules/identity/WebportSystem.Identity.Infrastructure/"]
COPY ["src/common/WebportSystem.Common.Infrastructure/WebportSystem.Common.Infrastructure.csproj", "src/common/WebportSystem.Common.Infrastructure/"]
COPY ["src/common/WebportSystem.Common.Application/WebportSystem.Common.Application.csproj", "src/common/WebportSystem.Common.Application/"]
COPY ["src/common/WebportSystem.Common.Domain/WebportSystem.Common.Domain.csproj", "src/common/WebportSystem.Common.Domain/"]
COPY ["src/modules/identity/WebportSystem.Identity.Application/WebportSystem.Identity.Application.csproj", "src/modules/identity/WebportSystem.Identity.Application/"]
COPY ["src/modules/identity/WebportSystem.Identity.Domain/WebportSystem.Identity.Domain.csproj", "src/modules/identity/WebportSystem.Identity.Domain/"]
COPY ["src/modules/identity/WebportSystem.Identity.Presentation/WebportSystem.Identity.Presentation.csproj", "src/modules/identity/WebportSystem.Identity.Presentation/"]
COPY ["src/common/WebportSystem.Common.Presentation/WebportSystem.Common.Presentation.csproj", "src/common/WebportSystem.Common.Presentation/"]
RUN dotnet restore "./src/api/WebportSystem.Api/WebportSystem.Api.csproj"
COPY . .
WORKDIR "/src/src/api/WebportSystem.Api"
RUN dotnet build "./WebportSystem.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WebportSystem.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebportSystem.Api.dll"]