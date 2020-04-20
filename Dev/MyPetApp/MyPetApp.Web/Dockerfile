FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["MyPetApp.Web/MyPetApp.Web.csproj", "MyPetApp.Web/"]
RUN dotnet restore "MyPetApp.Web/MyPetApp.Web.csproj"
COPY . .
WORKDIR "/src/MyPetApp.Web"
RUN dotnet build "MyPetApp.Web.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "MyPetApp.Web.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MyPetApp.Web.dll"]