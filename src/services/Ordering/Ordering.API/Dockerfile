#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["services/Ordering/Ordering.API/Ordering.API.csproj", "services/Ordering/Ordering.API/"]
COPY ["services/Ordering/Ordering.Application/Ordering.Application.csproj", "services/Ordering/Ordering.Application/"]
COPY ["services/Ordering/Ordering.Domain/Ordering.Domain.csproj", "services/Ordering/Ordering.Domain/"]
COPY ["services/Ordering/Ordering.Infrastructure/Ordering.Infrastructure.csproj", "services/Ordering/Ordering.Infrastructure/"]
COPY ["common/EventBus.Messages/EventBus.Messages.csproj", "common/EventBus.Messages/"]
RUN dotnet restore "services/Ordering/Ordering.API/Ordering.API.csproj"
COPY . .
WORKDIR "/src/services/Ordering/Ordering.API"
RUN dotnet build "Ordering.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ordering.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ordering.API.dll"]
