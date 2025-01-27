FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SorokChatServer.API/SorokChatServer.API.csproj", "SorokChatServer.API/"]
COPY ["SorokChatServer.Infrastructure/SorokChatServer.Infrastructure.csproj", "SorokChatServer.Infrastructure/"]
COPY ["SorokChatServer.DataAccess/SorokChatServer.DataAccess.csproj", "SorokChatServer.DataAccess/"]
COPY ["SorokChatServer.Core/SorokChatServer.Core.csproj", "SorokChatServer.Core/"]
RUN dotnet restore "SorokChatServer.API/SorokChatServer.API.csproj"
COPY . .
WORKDIR "/src/SorokChatServer.API"
RUN dotnet build "SorokChatServer.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SorokChatServer.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SorokChatServer.API.dll"]
