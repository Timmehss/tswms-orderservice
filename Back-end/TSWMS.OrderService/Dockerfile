# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TSWMS.OrderService.sln", "."]
COPY ["TSWMS.OrderService.Api/TSWMS.OrderService.Api.csproj", "TSWMS.OrderService.Api/"]
COPY ["TSWMS.OrderService.Business/TSWMS.OrderService.Business.csproj", "TSWMS.OrderService.Business/"]
COPY ["TSWMS.OrderService.Configurations/TSWMS.OrderService.Configurations.csproj", "TSWMS.OrderService.Configurations/"]
COPY ["TSWMS.OrderService.Data/TSWMS.OrderService.Data.csproj", "TSWMS.OrderService.Data/"]
COPY ["TSWMS.OrderService.Shared/TSWMS.OrderService.Shared.csproj", "TSWMS.OrderService.Shared/"]

RUN dotnet restore "TSWMS.OrderService.sln"

# Copy the remaining code and build the project
COPY . .
WORKDIR "/src/TSWMS.OrderService.Api"
RUN dotnet build "TSWMS.OrderService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TSWMS.OrderService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

EXPOSE 8080
EXPOSE 8081

# Final stage - run the application
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Development
ENTRYPOINT ["dotnet", "TSWMS.OrderService.Api.dll"]