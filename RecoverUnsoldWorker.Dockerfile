FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["RecoverUnsoldWorker/RecoverUnsoldWorker.csproj", "RecoverUnsoldWorker/"]
RUN dotnet restore "RecoverUnsoldWorker/RecoverUnsoldWorker.csproj"
COPY . .
WORKDIR "/src/RecoverUnsoldWorker"
RUN dotnet publish "RecoverUnsoldWorker.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "RecoverUnsoldWorker.dll"]