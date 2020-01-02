FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["TeamA.CustomerAccounts.API/TeamA.CustomerAccounts.API.csproj", "TeamA.CustomerAccounts.API/"]
COPY ["TeamA.CustomerAccounts.Models/TeamA.CustomerAccounts.Models.csproj", "TeamA.CustomerAccounts.Models/"]
COPY ["TeamA.CustomerAccounts.Data/TeamA.CustomerAccounts.Data.csproj", "TeamA.CustomerAccounts.Data/"]
COPY ["TeamA.CustomerAccounts.Services/TeamA.CustomerAccounts.Services.csproj", "TeamA.CustomerAccounts.Services/"]
COPY ["TeamA.CustomerAccounts.Repository/TeamA.CustomerAccounts.Repository.csproj", "TeamA.CustomerAccounts.Repository/"]
RUN dotnet restore "TeamA.CustomerAccounts.API/TeamA.CustomerAccounts.API.csproj"
COPY . .
WORKDIR "/src/TeamA.CustomerAccounts.API"
RUN dotnet build "TeamA.CustomerAccounts.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "TeamA.CustomerAccounts.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "TeamA.CustomerAccounts.API.dll"]