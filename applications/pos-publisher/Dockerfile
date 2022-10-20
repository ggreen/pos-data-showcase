# FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base
# WORKDIR /app
# EXPOSE 80

# FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
# WORKDIR /src
# COPY ["pos-publisher.csproj", "."]
# RUN dotnet restore "pos-publisher.csproj"
# COPY . .
# RUN dotnet build "pos-publisher.csproj" -c Release -o /app

# FROM build AS publish
# RUN dotnet publish "pos-publisher.csproj" -c Release -o /app

# FROM base AS final
# WORKDIR /app
# docker 
# ENV DOTNET_URLS http://0.0.0.0:80
# ENTRYPOINT ["dotnet", "pos-publisher.dll"]



# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app
    
# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore
    
# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out
    
# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "pos-publisher.dll"]