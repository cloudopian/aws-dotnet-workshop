FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["MyWolfApp.csproj", "./"]
RUN dotnet restore "./MyWolfApp.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "MyWolfApp.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "MyWolfApp.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MyWolfApp.dll"]
