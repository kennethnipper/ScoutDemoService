FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["ScoutDemoService.Core/ScoutDemoService.Core.csproj", "ScoutDemoService.Core/"]
RUN dotnet restore "ScoutDemoService.Core/ScoutDemoService.Core.csproj"
COPY . .
WORKDIR "/src/ScoutDemoService.Core"
RUN dotnet build "ScoutDemoService.Core.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ScoutDemoService.Core.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ScoutDemoService.Core.dll"]