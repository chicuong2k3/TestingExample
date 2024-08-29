# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ["Customers.Api/Customers.Api.csproj", "Customers.Api/"]
RUN dotnet restore "Customers.Api/Customers.Api.csproj"

# Copy everything else and build
COPY . .
WORKDIR /src/Customers.Api
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Serve
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/publish .
COPY cert.pfx /https/cert.pfx
ENTRYPOINT ["dotnet", "Customers.Api.dll"]
