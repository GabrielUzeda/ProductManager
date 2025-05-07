FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/API/ProductManager.API/ProductManager.API.csproj", "src/API/ProductManager.API/"]
COPY ["src/Application/ProductManager.Application/ProductManager.Application.csproj", "src/Application/ProductManager.Application/"]
COPY ["src/Domain/ProductManager.Domain/ProductManager.Domain.csproj", "src/Domain/ProductManager.Domain/"]
COPY ["src/Infrastructure/ProductManager.Infrastructure/ProductManager.Infrastructure.csproj", "src/Infrastructure/ProductManager.Infrastructure/"]
RUN dotnet restore "src/API/ProductManager.API/ProductManager.API.csproj"
COPY . .
WORKDIR "/src/src/API/ProductManager.API"
RUN dotnet build "ProductManager.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProductManager.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:80;https://+:443
ENTRYPOINT ["dotnet", "ProductManager.API.dll"]
