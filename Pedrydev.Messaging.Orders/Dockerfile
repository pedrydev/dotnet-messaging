FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Pedrydev.Messaging.Orders/Pedrydev.Messaging.Orders.csproj", "Pedrydev.Messaging.Orders/"]
RUN dotnet restore "Pedrydev.Messaging.Orders/Pedrydev.Messaging.Orders.csproj"
COPY . .
WORKDIR "/src/Pedrydev.Messaging.Orders"
RUN dotnet build "Pedrydev.Messaging.Orders.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Pedrydev.Messaging.Orders.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pedrydev.Messaging.Orders.dll"]
