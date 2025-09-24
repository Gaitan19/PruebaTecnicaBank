# PruebaTecnicaBank

## Descripción
Sistema bancario desarrollado en .NET 8 con arquitectura en capas que incluye operaciones de clientes y cuentas bancarias con sus respectivas transacciones.

## Estructura del Proyecto

```
PruebaTecnicaBank/
├── PruebaTecnicaBank.Api/          # Capa de presentación (Web API)
├── PruebaTecnicaBank.Core/         # Capa de dominio (Entities, DTOs, Services, Interfaces)
├── PruebaTecnicaBank.Infrastructure/ # Capa de infraestructura (Repositories, Context, Mappings)
├── PruebaTecnicaBank.Tests/        # Pruebas unitarias
└── PruebaTecnicaBank.sln          # Archivo de solución
```

## Funcionalidades Implementadas

### ClienteServicio
- ✅ Crear clientes
- ✅ Obtener cliente por ID
- ✅ Obtener todos los clientes

### CuentaServicio
- ✅ Crear cuentas bancarias
- ✅ Realizar depósitos
- ✅ Realizar retiros
- ✅ Aplicar intereses
- ✅ Consultar saldo
- ✅ Obtener historial de transacciones

## 🚀 Instrucciones para Ejecutar el Proyecto y las Pruebas

### Comandos Rápidos

```bash
# 1. Clonar el repositorio
git clone https://github.com/Gaitan19/PruebaTecnicaTest.git
cd PruebaTecnicaTest

# 2. Restaurar dependencias
dotnet restore

# 3. Ejecutar las pruebas unitarias
dotnet test

# 4. Compilar el proyecto
dotnet build

# 5. Ejecutar la API
cd PruebaTecnicaBank.Api
dotnet run
```

**La API estará disponible en:**

- Swagger UI: `/swagger`

## Requisitos Previos

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Visual Studio 2022 / Visual Studio Code / JetBrains Rider (opcional)
- **Entity Framework Tools** (instalar si no está disponible):
  ```bash
  dotnet tool install --global dotnet-ef
  ```

## Instalación

1. **Clonar el repositorio:**
   ```bash
   git clone https://github.com/Gaitan19/PruebaTecnicaTest.git
   cd PruebaTecnicaTest
   ```

2. **Restaurar paquetes NuGet:**
   ```bash
   dotnet restore
   ```

3. **Compilar la solución:**
   ```bash
   dotnet build
   ```

## 🗄️ Gestión de Base de Datos y Migraciones

Este proyecto utiliza **Entity Framework Core** con **SQLite** para el manejo de la base de datos. A continuación se detallan los comandos para crear y gestionar migraciones.

### Comandos para Migraciones

#### 1. Crear la migración inicial
```bash
dotnet ef migrations add InitialCreate --project PruebaTecnicaBank.Infrastructure --startup-project PruebaTecnicaBank.Api
```

#### 2. Agregar nuevas migraciones (después de cambios en el modelo)
```bash
dotnet ef migrations add NombreDeLaMigracion --project PruebaTecnicaBank.Infrastructure --startup-project PruebaTecnicaBank.Api
```

#### 3. Actualizar la base de datos
```bash
dotnet ef database update --project PruebaTecnicaBank.Infrastructure --startup-project PruebaTecnicaBank.Api
```

## Ejecución de Pruebas Unitarias

### 🔧 Comandos Paso a Paso

#### 1. Restaurar dependencias del proyecto de pruebas
```bash
dotnet restore PruebaTecnicaBank.Tests/PruebaTecnicaBank.Tests.csproj
```

#### 2. Compilar las pruebas
```bash
dotnet build PruebaTecnicaBank.Tests/PruebaTecnicaBank.Tests.csproj
```

#### 3. Ejecutar todas las pruebas unitarias
```bash
dotnet test
```

#### 4. Ejecutar pruebas con información detallada
```bash
dotnet test --verbosity normal
```

#### 5. Ejecutar pruebas mostrando solo resultados
```bash
dotnet test --verbosity minimal
```

#### 6. Listar todas las pruebas disponibles sin ejecutarlas
```bash
dotnet test --list-tests
```

#### 7. Ejecutar solo las pruebas de ClienteServicio
```bash
dotnet test --filter "FullyQualifiedName~ClienteServicioTests"
```

#### 8. Ejecutar solo las pruebas de CuentaServicio
```bash
dotnet test --filter "FullyQualifiedName~CuentaServicioTests"
```

#### 9. Ejecutar una prueba específica
```bash
dotnet test --filter "FullyQualifiedName~CrearClienteAsync_DatosValidos_RetornaClienteCreado"
```

### 📈 Comandos de Cobertura de Código

#### Comando principal para cobertura detallada (recomendado)
```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=lcov
```

Este comando genera una tabla detallada con porcentajes de cobertura enfocada solo en el código que estamos probando:
```
+------------------------+--------+--------+--------+
| Module                 | Line   | Branch | Method |
+------------------------+--------+--------+--------+
| PruebaTecnicaBank.Core | 92.81% | 83.33% | 92.85% |
+------------------------+--------+--------+--------+

+---------+--------+--------+--------+
|         | Line   | Branch | Method |
+---------+--------+--------+--------+
| Total   | 92.81% | 83.33% | 92.85% |
+---------+--------+--------+--------+
```

### 🎯 Ejemplo de Salida Esperada

```
Test run for PruebaTecnicaBank.Tests.dll (.NETCoreApp,Version=v8.0)
Microsoft (R) Test Execution Command Line Tool

Starting test execution, please wait...

Passed!  - Failed:     0, Passed:    24, Skipped:     0, Total:    24, Duration: < 1s
```

### 🛠️ Tecnologías de Pruebas Utilizadas

- **xUnit:** Framework de pruebas unitarias
- **Moq:** Biblioteca para crear objetos mock
- **AutoMapper:** Mapeo automático de objetos
- **Microsoft.EntityFrameworkCore.InMemory:** Base de datos en memoria para pruebas
- **Coverlet:** Herramienta de cobertura de código multiplataforma para .NET

### 🚀 Ejecución de la API

Para ejecutar la API web:

```bash
cd PruebaTecnicaBank.Api
dotnet run
```

### 📁 Archivos de Pruebas

- `CuentaServicioTests.cs` - Pruebas para el servicio de cuentas bancarias
- `ClienteServicioTests.cs` - Pruebas para el servicio de clientes

