FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY src/FornecedoresApp.Domain/FornecedoresApp.Domain.csproj src/FornecedoresApp.Domain/
COPY src/FornecedoresApp.Infrastructure/FornecedoresApp.Infrastructure.csproj src/FornecedoresApp.Infrastructure/
COPY src/FornecedoresApp.API/FornecedoresApp.API.csproj src/FornecedoresApp.API/
RUN dotnet restore src/FornecedoresApp.API/FornecedoresApp.API.csproj

COPY src/ src/
WORKDIR /src/src/FornecedoresApp.API
RUN dotnet publish FornecedoresApp.API.csproj --configuration Release --no-restore --output /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080
USER $APP_UID

ENTRYPOINT ["dotnet", "FornecedoresApp.API.dll"]
