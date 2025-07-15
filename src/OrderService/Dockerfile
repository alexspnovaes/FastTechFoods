# Acesse https://aka.ms/customizecontainer para saber como personalizar seu contêiner de depuração e como o Visual Studio usa este Dockerfile para criar suas imagens para uma depuração mais rápida.

# Esta fase é usada durante a execução no VS no modo rápido (Padrão para a configuração de Depuração)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8085
EXPOSE 8086


# Esta fase é usada para compilar o projeto de serviço
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/OrderService/FastTechFoods.OrderService.API/FastTechFoods.OrderService.API.csproj", "src/OrderService/FastTechFoods.OrderService.API/"]
COPY ["src/BuildingBlocks/FastTechFoods.BuildingBlocks.Security/FastTechFoods.BuildingBlocks.Security.csproj", "src/BuildingBlocks/FastTechFoods.BuildingBlocks.Security/"]
COPY ["src/OrderService/FastTechFoods.OrderService.Application/FastTechFoods.OrderService.Application.csproj", "src/OrderService/FastTechFoods.OrderService.Application/"]
COPY ["src/BuildingBlocks/FastTechFoods.BuildingBlocks.Messaging/FastTechFoods.BuildingBlocks.Messaging.csproj", "src/BuildingBlocks/FastTechFoods.BuildingBlocks.Messaging/"]
COPY ["src/OrderService/FastTechFoods.OrderService.Domain/FastTechFoods.OrderService.Domain.csproj", "src/OrderService/FastTechFoods.OrderService.Domain/"]
COPY ["src/OrderService/FastTechFoods.OrderService.Infrastructure/FastTechFoods.OrderService.Infrastructure.csproj", "src/OrderService/FastTechFoods.OrderService.Infrastructure/"]
RUN dotnet restore "./src/OrderService/FastTechFoods.OrderService.API/FastTechFoods.OrderService.API.csproj"
COPY . .
WORKDIR "/src/src/OrderService/FastTechFoods.OrderService.API"
RUN dotnet build "./FastTechFoods.OrderService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Esta fase é usada para publicar o projeto de serviço a ser copiado para a fase final
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./FastTechFoods.OrderService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Esta fase é usada na produção ou quando executada no VS no modo normal (padrão quando não está usando a configuração de Depuração)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FastTechFoods.OrderService.API.dll"]