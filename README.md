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
- HTTP: `http://localhost:5000`
- HTTPS: `https://localhost:5001` 
- Swagger UI: `https://localhost:5001/swagger`

## Requisitos Previos

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Visual Studio 2022 / Visual Studio Code / JetBrains Rider (opcional)

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

**Nota:** La configuración excluye automáticamente `PruebaTecnicaBank.Infrastructure` del análisis de cobertura ya que no estamos probando la capa de infraestructura, solo los servicios del dominio.

#### Otras opciones de cobertura:
```bash
# Cobertura básica (genera archivo XML)
dotnet test --collect:"XPlat Code Coverage"

# Múltiples formatos
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat="lcov,opencover,cobertura"

# Solo mostrar resumen
dotnet test /p:CollectCoverage=true
```

### 📊 Cobertura de Pruebas

#### ClienteServicio (13 pruebas)
- ✅ **Creación de clientes:** 4 pruebas
  - Datos válidos masculino/femenino
  - Ingresos mínimos
  - Nombres largos
- ✅ **Obtención de cliente:** 3 pruebas
  - Cliente existente
  - Cliente no existente
  - Cliente con cuentas asociadas
- ✅ **Obtención de todos los clientes:** 3 pruebas
  - Lista con clientes
  - Lista vacía
  - Clientes con diferentes sexos
- ✅ **Casos extremos:** 3 pruebas
  - Fechas antiguas
  - Ingresos máximos
  - GUID vacío

#### CuentaServicio (18 pruebas)
- ✅ **Creación de cuentas:** 3 pruebas
- ✅ **Operaciones de depósito:** 3 pruebas
- ✅ **Operaciones de retiro:** 3 pruebas
- ✅ **Aplicación de intereses:** 3 pruebas
- ✅ **Consulta de saldo:** 2 pruebas
- ✅ **Historial de transacciones:** 2 pruebas
- ✅ **Casos extremos:** 3 pruebas

### 🎯 Ejemplo de Salida Esperada

```
Test run for PruebaTecnicaBank.Tests.dll (.NETCoreApp,Version=v8.0)
Microsoft (R) Test Execution Command Line Tool

Starting test execution, please wait...

Passed!  - Failed:     0, Passed:    31, Skipped:     0, Total:    31, Duration: < 1s
```

### 🛠️ Tecnologías de Pruebas Utilizadas

- **xUnit:** Framework de pruebas unitarias
- **Moq:** Biblioteca para crear objetos mock
- **AutoMapper:** Mapeo automático de objetos
- **Microsoft.EntityFrameworkCore.InMemory:** Base de datos en memoria para pruebas
- **Coverlet:** Herramienta de cobertura de código multiplataforma para .NET

### 📝 Convenciones de Nomenclatura de Pruebas

Las pruebas siguen la convención: `[Método]_[Escenario]_[ResultadoEsperado]`

Ejemplos:
- `CrearClienteAsync_DatosValidos_RetornaClienteCreado`
- `DepositarAsync_CuentaNoExiste_LanzaExcepcion`
- `ObtenerClienteAsync_ClienteExiste_RetornaCliente`

### 🚀 Ejecución de la API

Para ejecutar la API web:

```bash
cd PruebaTecnicaBank.Api
dotnet run
```

La API estará disponible en:
- HTTP: `http://localhost:5000`
- HTTPS: `https://localhost:5001`
- Swagger UI: `https://localhost:5001/swagger`

### 📁 Archivos de Pruebas

- `CuentaServicioTests.cs` - Pruebas para el servicio de cuentas bancarias
- `ClienteServicioTests.cs` - Pruebas para el servicio de clientes

### ✅ Verificación Rápida

Para verificar que todo funciona correctamente:

```bash
# 1. Compilar
dotnet build

# 2. Ejecutar pruebas
dotnet test

# 3. Ejecutar pruebas con cobertura detallada
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=lcov

# 4. Si todo está bien, deberías ver algo como:
# Passed!  - Failed: 0, Passed: 31, Skipped: 0, Total: 31
# + tabla de cobertura con porcentajes por módulo
```