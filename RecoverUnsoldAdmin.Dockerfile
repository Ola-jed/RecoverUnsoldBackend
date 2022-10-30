FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["RecoverUnsoldAdmin/RecoverUnsoldAdmin.csproj", "RecoverUnsoldAdmin/"]
RUN dotnet restore "RecoverUnsoldAdmin/RecoverUnsoldAdmin.csproj"
COPY . .
WORKDIR "/src/RecoverUnsoldAdmin"
RUN dotnet build "RecoverUnsoldAdmin.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RecoverUnsoldAdmin.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RecoverUnsoldAdmin.dll"]
