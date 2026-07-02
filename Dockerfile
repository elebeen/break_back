# =========================================================
# Etapa 1: Entorno de ejecución (Runtime)
# =========================================================
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app

# Muchos proveedores en la nube asignan un puerto dinámico mediante la variable de entorno PORT.
# Configuramos ASP.NET Core para que escuche en cualquier IP (0.0.0.0) usando el puerto por defecto (8080)
# o el que asigne el proveedor (puedes cambiarlo a 10000 si tu proveedor lo exige explícitamente).
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# =========================================================
# Etapa 2: Entorno de compilación (SDK)
# =========================================================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# 1. Copiar la solución y los archivos de proyecto (.csproj) individuales
# Esto se hace primero para que Docker guarde en caché el paso de restauración de NuGet.
COPY ["break_back.sln", "./"]
COPY ["break_back/break_back.csproj", "break_back/"]
COPY ["nutria.Application/nutria.Application.csproj", "nutria.Application/"]
COPY ["Nutria.Domain/Nutria.Domain.csproj", "Nutria.Domain/"]
COPY ["Nutria.Infrastructure/Nutria.Infrastructure.csproj", "Nutria.Infrastructure/"]

# 2. Restaurar todas las dependencias de NuGet de la solución completa
RUN dotnet restore "break_back.sln"

# 3. Copiar el resto de los archivos fuentes del código
COPY . .

# 4. Cambiar el directorio de trabajo al proyecto ejecutable principal (la Web API)
WORKDIR "/src/break_back"

# 5. Compilar la API en modo de optimización (Release)
RUN dotnet build "break_back.csproj" -c Release -o /app/build

# =========================================================
# Etapa 3: Publicación de la aplicación
# =========================================================
FROM build AS publish
RUN dotnet publish "break_back.csproj" -c Release -o /app/publish /p:UseAppHost=false

# =========================================================
# Etapa 4: Imagen final ligera orientada a producción
# =========================================================
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Comando de inicio de tu contenedor invocando la DLL principal de la API
ENTRYPOINT ["dotnet", "break_back.dll"]