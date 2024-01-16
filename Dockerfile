FROM mcr.microsoft.com/dotnet/aspnet:7.0.14-jammy-amd64 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ResourceLoader/ResourceLoader.csproj", "ResourceLoader/"]
RUN dotnet restore "ResourceLoader/ResourceLoader.csproj"
COPY . .
WORKDIR "/src/ResourceLoader"
RUN dotnet build "ResourceLoader.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ResourceLoader.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ResourceLoader.dll"]