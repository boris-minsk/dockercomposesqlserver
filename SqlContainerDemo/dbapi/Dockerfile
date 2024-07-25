#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 443
EXPOSE 58441

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["dbapi/dbapi.csproj", "dbapi/"]
RUN dotnet restore "./dbapi/./dbapi.csproj"
COPY . .
WORKDIR "/src/dbapi"
RUN dotnet build "./dbapi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./dbapi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

HEALTHCHECK --interval=60s --timeout=10s --retries=2 CMD curl --fail http://localhost:58441/health #|| exit 1

ENTRYPOINT ["dotnet", "dbapi.dll"]
