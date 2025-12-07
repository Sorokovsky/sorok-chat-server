FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SorokChatServer.Application/SorokChatServer.Application.csproj", "SorokChatServer.Application/"]
COPY ["SorokChatServer.Domain/SorokChatServer.Domain.csproj", "SorokChatServer.Domain/"]
COPY ["SorokChatServer.Persistence.Postgres/SorokChatServer.Persistence.Postgres.csproj", "SorokChatServer.Persistence.Postgres/"]
RUN dotnet restore "SorokChatServer.Application/SorokChatServer.Application.csproj"
COPY . .
WORKDIR "/src/SorokChatServer.Application"
RUN dotnet build "./SorokChatServer.Application.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SorokChatServer.Application.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SorokChatServer.Application.dll"]
