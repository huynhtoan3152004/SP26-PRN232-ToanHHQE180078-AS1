# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["HuynhHuuToan_ SE1856_A01_BE/HuynhHuuToan_ SE1856_A01_BE.csproj", "HuynhHuuToan_ SE1856_A01_BE/"]
COPY ["HuynhHuuToan_ SE1856_A01_Service/HuynhHuuToan_ SE1856_A01_Service.csproj", "HuynhHuuToan_ SE1856_A01_Service/"]
COPY ["HuynhHuuToan_ SE1856_A01_Repository/HuynhHuuToan_ SE1856_A01_Repository.csproj", "HuynhHuuToan_ SE1856_A01_Repository/"]
RUN dotnet restore "HuynhHuuToan_ SE1856_A01_BE/HuynhHuuToan_ SE1856_A01_BE.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/HuynhHuuToan_ SE1856_A01_BE"
RUN dotnet build "HuynhHuuToan_ SE1856_A01_BE.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "HuynhHuuToan_ SE1856_A01_BE.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HuynhHuuToan_ SE1856_A01_BE.dll"]
