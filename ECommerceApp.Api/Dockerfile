# Base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Projeleri kopyala
COPY ["ECommerceApp.Api/ECommerceApp.Api.csproj", "ECommerceApp.Api/"]
COPY ["ECommerceApp.Application/ECommerceApp.Application.csproj", "ECommerceApp.Application/"]
COPY ["ECommerceApp.Domain/ECommerceApp.Domain.csproj", "ECommerceApp.Domain/"]
COPY ["ECommerceApp.Infrastructure/ECommerceApp.Infrastructure.csproj", "ECommerceApp.Infrastructure/"]
COPY ["ECommerceApp.Persistence/ECommerceApp.Persistence.csproj", "ECommerceApp.Persistence/"]


# Restore bağımlılıklar
RUN dotnet restore "ECommerceApp.Api/ECommerceApp.Api.csproj"

# Diğer dosyaları kopyala ve build et
COPY . .
WORKDIR "/src/ECommerceApp.Api"
RUN dotnet build "ECommerceApp.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ECommerceApp.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final container
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ECommerceApp.Api.dll"]
