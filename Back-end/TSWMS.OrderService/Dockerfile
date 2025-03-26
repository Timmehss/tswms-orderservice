# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy the solution file and all the project files
COPY ["TSWMS.OrderService.sln", "."]
COPY ["TSWMS.OrderService.Api/TSWMS.OrderService.Api.csproj", "TSWMS.OrderService.Api/"]
COPY ["TSWMS.OrderService.Business/TSWMS.OrderService.Business.csproj", "TSWMS.OrderService.Business/"]
COPY ["TSWMS.OrderService.Configurations/TSWMS.OrderService.Configurations.csproj", "TSWMS.OrderService.Configurations/"]
COPY ["TSWMS.OrderService.Data/TSWMS.OrderService.Data.csproj", "TSWMS.OrderService.Data/"]
COPY ["TSWMS.OrderService.Shared/TSWMS.OrderService.Shared.csproj", "TSWMS.OrderService.Shared/"]

# Copy the test projects
COPY ["Tests/TSWMS.OrderService.Business.UnitTests/TSWMS.OrderService.Business.UnitTests.csproj", "Tests/TSWMS.OrderService.Business.UnitTests/"]
COPY ["Tests/TSWMS.OrderService.Data.UnitTests/TSWMS.OrderService.Data.UnitTests.csproj", "Tests/TSWMS.OrderService.Data.UnitTests/"]
COPY ["Tests/TSWMS.OrderService.Api.IntegrationTests/TSWMS.OrderService.Api.IntegrationTests.csproj", "Tests/TSWMS.OrderService.Api.IntegrationTests/"]

# Restore all the dependencies
RUN dotnet restore "TSWMS.OrderService.sln"

# Copy the rest of the code
COPY . . 

# Build the application
WORKDIR "/src/TSWMS.OrderService.Api"
RUN dotnet build "TSWMS.OrderService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the API project
FROM build AS publish
RUN dotnet publish "TSWMS.OrderService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

EXPOSE 8080
EXPOSE 8081

# Final stage - run the application
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TSWMS.OrderService.Api.dll"]
