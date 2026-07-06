FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/Pedidos.Api/Pedidos.Api.csproj", "src/Pedidos.Api/"]
COPY ["src/Pedidos.Application/Pedidos.Application.csproj", "src/Pedidos.Application/"]
COPY ["src/Pedidos.Domain/Pedidos.Domain.csproj", "src/Pedidos.Domain/"]
COPY ["src/Pedidos.Infrastructure/Pedidos.Infrastructure.csproj", "src/Pedidos.Infrastructure/"]
RUN dotnet restore "src/Pedidos.Api/Pedidos.Api.csproj"
COPY . .
WORKDIR "/src/src/Pedidos.Api"
RUN dotnet build "Pedidos.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Pedidos.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pedidos.Api.dll"]
