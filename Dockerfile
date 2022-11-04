#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["simple-SQLite-manipulator.cs/simple-SQLite-manipulator.cs.csproj", "simple-SQLite-manipulator.cs/"]
RUN dotnet restore "simple-SQLite-manipulator.cs/simple-SQLite-manipulator.cs.csproj"
COPY . .
WORKDIR "/src/simple-SQLite-manipulator.cs"
RUN dotnet build "simple-SQLite-manipulator.cs.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "simple-SQLite-manipulator.cs.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "simple-SQLite-manipulator.cs.dll"]
