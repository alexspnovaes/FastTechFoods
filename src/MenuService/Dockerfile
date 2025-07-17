# Acesse https://aka.ms/customizecontainer para saber como personalizar seu contêiner de depuração e como o Visual Studio usa este Dockerfile para criar suas imagens para uma depuração mais rápida.

# Esta fase é usada durante a execução no VS no modo rápido (Padrão para a configuração de Depuração)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8083
EXPOSE 8084


# Esta fase é usada para compilar o projeto de serviço
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/MenuService/FastTechFoods.MenuService.API/FastTechFoods.MenuService.API.csproj", "src/MenuService/FastTechFoods.MenuService.API/"]
COPY ["src/BuildingBlocks/FastTechFoods.BuildingBlocks.Security/FastTechFoods.BuildingBlocks.Security.csproj", "src/BuildingBlocks/FastTechFoods.BuildingBlocks.Security/"]
COPY ["src/MenuService/FastTechFoods.MenuService.Application/FastTechFoods.MenuService.Application.csproj", "src/MenuService/FastTechFoods.MenuService.Application/"]
COPY ["src/MenuService/FastTechFoods.MenuService.Domain/FastTechFoods.MenuService.Domain.csproj", "src/MenuService/FastTechFoods.MenuService.Domain/"]
COPY ["src/MenuService/FastTechFoods.MenuService.Infrastructure/FastTechFoods.MenuService.Infrastructure.csproj", "src/MenuService/FastTechFoods.MenuService.Infrastructure/"]
RUN dotnet restore "./src/MenuService/FastTechFoods.MenuService.API/FastTechFoods.MenuService.API.csproj"
COPY . .
WORKDIR "/src/src/MenuService/FastTechFoods.MenuService.API"
RUN dotnet build "./FastTechFoods.MenuService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Esta fase é usada para publicar o projeto de serviço a ser copiado para a fase final
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./FastTechFoods.MenuService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Esta fase é usada na produção ou quando executada no VS no modo normal (padrão quando não está usando a configuração de Depuração)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FastTechFoods.MenuService.API.dll"]