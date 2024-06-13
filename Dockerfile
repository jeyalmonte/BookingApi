FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

ARG BUILD_CONFIGURATION=Release
COPY . .
RUN dotnet restore 
RUN dotnet publish -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Api.dll"]