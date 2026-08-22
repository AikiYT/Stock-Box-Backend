FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src

COPY ["StockBox.Api/StockBox.Api.csproj", "StockBox.Api/"]
COPY ["StockBox.Application/StockBox.Application.csproj", "StockBox.Application/"]
COPY ["Stock-Box/StockBox.Domain.csproj", "Stock-Box/"]
COPY ["StockBox.Infrastructure/StockBox.Infrastructure.csproj", "StockBox.Infrastructure/"]
COPY ["StockBox.Identity/StockBox.Identity.csproj", "StockBox.Identity/"]

RUN dotnet restore "StockBox.Api/StockBox.Api.csproj"

COPY . .

WORKDIR "/src/StockBox.Api"

RUN dotnet build "StockBox.Api.csproj" -c Release -o /app/build

FROM build AS publish

RUN dotnet publish "StockBox.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final

WORKDIR /app

COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080

ENTRYPOINT ["dotnet", "StockBox.Api.dll"]