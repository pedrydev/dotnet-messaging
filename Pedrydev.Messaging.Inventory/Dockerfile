FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Pedrydev.Messaging.Inventory/Pedrydev.Messaging.Inventory.csproj", "Pedrydev.Messaging.Inventory/"]
RUN dotnet restore "Pedrydev.Messaging.Inventory/Pedrydev.Messaging.Inventory.csproj"
COPY . .
WORKDIR "/src/Pedrydev.Messaging.Inventory"
RUN dotnet build "Pedrydev.Messaging.Inventory.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Pedrydev.Messaging.Inventory.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pedrydev.Messaging.Inventory.dll"]
