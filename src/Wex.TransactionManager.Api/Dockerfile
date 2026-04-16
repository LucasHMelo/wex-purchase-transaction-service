FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Wex.TransactionManager.Api/Wex.TransactionManager.Api.csproj", "src/Wex.TransactionManager.Api/"]
RUN dotnet restore "src/Wex.TransactionManager.Api/Wex.TransactionManager.Api.csproj"
COPY . .
WORKDIR "/src/src/Wex.TransactionManager.Api"
RUN dotnet build "Wex.TransactionManager.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Wex.TransactionManager.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Wex.TransactionManager.Api.dll"]