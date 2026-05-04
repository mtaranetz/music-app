# ====== STAGE 1: build ======
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# копируем csproj и восстанавливаем зависимости
COPY *.csproj ./
RUN dotnet restore

# копируем всё остальное и билдим
COPY . ./
RUN dotnet publish -c Release -o /publish

# ====== STAGE 2: runtime ======
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# копируем билд
COPY --from=build /publish .

# порт (ВАЖНО для Render)
ENV ASPNETCORE_URLS=http://+:10000

EXPOSE 10000

ENTRYPOINT ["dotnet", "MusicApp.dll"]