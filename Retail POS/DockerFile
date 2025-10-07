# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore separately (better layer caching)
COPY ["Retail_POS.csproj", "./"]
RUN dotnet restore "Retail_POS.csproj"

# Copy rest of the files and publish
COPY . .
RUN dotnet publish "Retail_POS.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Retail_POS.dll"]
