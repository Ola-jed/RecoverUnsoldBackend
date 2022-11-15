FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["RecoverUnsoldApi/RecoverUnsoldApi.csproj", "RecoverUnsoldApi/"]
RUN dotnet restore "RecoverUnsoldApi/RecoverUnsoldApi.csproj"
COPY . .
WORKDIR "/src/RecoverUnsoldApi"
RUN dotnet build "RecoverUnsoldApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RecoverUnsoldApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RecoverUnsoldApi.dll"]
