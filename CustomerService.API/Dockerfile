FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["CustomerService.API/CustomerService.API.csproj", "CustomerService.API/"]
COPY ["CustomerService.Application/CustomerService.Application.csproj", "CustomerService.Application/"]
COPY ["CustomerService.Domain/CustomerService.Domain.csproj", "CustomerService.Domain/"]
COPY ["CustomerService.Infrastructure/CustomerService.Infrastructure.csproj", "CustomerService.Infrastructure/"]
COPY ["Shared.Contracts/Shared.Contracts.csproj", "Shared.Contracts/"]

RUN dotnet restore "CustomerService.API/CustomerService.API.csproj"

COPY . .

WORKDIR "/src/CustomerService.API"

RUN dotnet build "CustomerService.API.csproj" \
    -c Release \
    -o /app/build

FROM build AS publish

RUN dotnet publish "CustomerService.API.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

FROM base AS final

WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "CustomerService.API.dll"]