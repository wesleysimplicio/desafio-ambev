# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Sales.WebApi/Sales.WebApi.csproj", "src/Sales.WebApi/"]
COPY ["src/Sales.Data/Sales.Data.csproj", "src/Sales.Data/"]
COPY ["src/Sales.Domain/Sales.Domain.csproj", "src/Sales.Domain/"]
RUN dotnet restore "./src/Sales.WebApi/Sales.WebApi.csproj"
COPY . .
WORKDIR "/src/src/Sales.WebApi"
RUN dotnet build "./Sales.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Sales.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Sales.WebApi.dll"]