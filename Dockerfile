#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

RUN apt-get update \
    && apt-get install -y curl
    
HEALTHCHECK --interval=30s --timeout=10s CMD curl -f http://localhost:80/healthz

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AdvancedForms/AdvancedForms.csproj", "AdvancedForms/"]
RUN dotnet restore "AdvancedForms/AdvancedForms.csproj"
COPY . .
WORKDIR "/src/AdvancedForms"
RUN dotnet build "AdvancedForms.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AdvancedForms.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AdvancedForms.dll"]
