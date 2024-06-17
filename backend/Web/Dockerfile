# Etapa de construcción
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src

# Copiar archivos de proyecto y restaurar dependencias
COPY ["Web/Web.csproj", "Web/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Shared/Shared.csproj", "Shared/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]

RUN dotnet restore "Web/Web.csproj"

# Copiar el resto de los archivos y construir
COPY . .
WORKDIR "/src/Web"
RUN dotnet build "Web.csproj" -c Release -o /app/build

# Publicar la aplicación
RUN dotnet publish "Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa final para la imagen de ejecución
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:4000
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

COPY --from=build /app/publish .
COPY --from=build /src/Infrastructure/Seeders /app/Seeders

RUN mkdir -p /app/ailogs
RUN addgroup --system nonroot && adduser --system --ingroup nonroot nonroot
USER nonroot

EXPOSE 4000
ENTRYPOINT ["dotnet", "Web.dll"]
