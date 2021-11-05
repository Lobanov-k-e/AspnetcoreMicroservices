#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["services/Discount/Discount.grpc/Discount.grpc.csproj", "services/Discount/Discount.grpc/"]
RUN dotnet restore "services/Discount/Discount.grpc/Discount.grpc.csproj"
COPY . .
WORKDIR "/src/services/Discount/Discount.grpc"
RUN dotnet build "Discount.grpc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Discount.grpc.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Discount.grpc.dll"]
