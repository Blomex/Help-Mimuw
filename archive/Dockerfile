FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["archive/archive.csproj", "archive/"]
RUN dotnet restore "archive/archive.csproj"
COPY . .
WORKDIR "/src/archive"
RUN dotnet build "archive.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "archive.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet archive.dll # As per Heroku documentation
