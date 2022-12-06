FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SupplyStacks.csproj", "./"]
RUN dotnet restore "SupplyStacks.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "SupplyStacks.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SupplyStacks.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SupplyStacks.dll"]
