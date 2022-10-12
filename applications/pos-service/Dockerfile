FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
WORKDIR /src
COPY ["pos_service.csproj", "."]
RUN dotnet restore "pos_service.csproj"
COPY . .
RUN dotnet build "pos_service.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "pos_service.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENV DOTNET_URLS http://0.0.0.0:80
ENTRYPOINT ["dotnet", "pos_service.dll"]
