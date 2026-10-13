# SmartBusinessPro-EntityFrameworkCore

Proyecto académico desarrollado en **.NET 10** y **C#** para la gestión y consulta de inventario/productos utilizando dos enfoques de acceso a datos: **ADO.NET clásico** y **Entity Framework Core 10**.

---

## 📁 Estructura de la Solución

La solución (`coleccion productos smarbussines.slnx`) se compone de dos proyectos:

```text
├── SmartBusinessPro.WinForms/            # Aplicación de escritorio (Windows Forms)
│   ├── Models/
│   │   ├── Categoria.cs                  # Entidad Categoria
│   │   ├── Producto.cs                   # Entidad Producto
│   │   └── SmartBusinessContext.cs       # DbContext de EF Core
│   ├── App.config                        # Cadena de conexión y configuración
│   ├── Form1.cs                          # Lógica del formulario e interfaz
│   ├── Form1.Designer.cs                 # Componentes visuales (DataGridView)
│   ├── Form1.resx                        # Recursos del formulario
│   ├── Program.cs                        # Punto de entrada de WinForms
│   └── SmartBusinessPro.WinForms.csproj
│
├── coleccion productos smarbussines/     # Aplicación de consola (ADO.NET)
│   ├── Program.cs                        # Consulta SQL directa con SqlDataReader
│   └── coleccion productos smarbussines.csproj
│
├── coleccion productos smarbussines.slnx # Archivo de solución de Visual Studio
├── .gitignore                            # Exclusiones de bin, obj y temporales
└── README.md                             # Documentación del proyecto
```

---

## 🛠️ Tecnologías y Dependencias

- **Framework**: .NET 10.0 / .NET 10.0-windows
- **Lenguaje**: C# 13 / C# 14
- **ORM / Acceso a Datos**:
  - `Microsoft.EntityFrameworkCore` (10.0.12)
  - `Microsoft.EntityFrameworkCore.SqlServer` (10.0.12)
  - `Microsoft.Data.SqlClient` (7.1.0)
  - `System.Configuration.ConfigurationManager` (10.0.12)
- **Interfaz Gráfica**: Windows Forms

---

## ⚙️ Configuración de Base de Datos y Cadena de Conexión

### 1. Requisitos de la Base de Datos
El proyecto requiere una base de datos en **SQL Server** con las tablas `Categorias` y `Productos`:

```sql
CREATE TABLE Categorias (
    CategoriaId INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL
);

CREATE TABLE Productos (
    ProductoId INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(150) NOT NULL,
    Precio DECIMAL(18,2) NOT NULL,
    Stock INT NOT NULL,
    Activo BIT NOT NULL DEFAULT 1,
    CategoriaId INT NOT NULL FOREIGN KEY REFERENCES Categorias(CategoriaId)
);
```

### 2. Cadena de Conexión
Para ejecutar el proyecto en su entorno local, configure la cadena de conexión en `SmartBusinessPro.WinForms/App.config`:

```xml
<configuration>
  <connectionStrings>
    <add name="SmartBusinessContext"
         connectionString="Server=localhost;Database=SmartBusinessPro;Integrated Security=True;TrustServerCertificate=True;"
         providerName="Microsoft.Data.SqlClient" />
  </connectionStrings>
</configuration>
```

> **Nota para el proyecto de consola**: La cadena de conexión también se encuentra definida en la variable `connString` dentro de `coleccion productos smarbussines/Program.cs`. Ajuste el nombre del servidor según su instancia local de SQL Server.

---

## 🚀 Cómo Ejecutar la Solución

1. Clonar el repositorio:
   ```bash
   git clone https://github.com/HeriJosue123/Casos-de-analisis-Creaci-n-de-contextos-y-cadenas-de-conexi-n-en-Entity-Framework-Core.git
   ```
2. Abrir la solución `coleccion productos smarbussines.slnx` en **Visual Studio 2022** (versión compatible con .NET 10) o compilar vía CLI:
   ```bash
   dotnet build "coleccion productos smarbussines.slnx"
   ```
3. Ejecutar el proyecto deseado:
   - **Windows Forms**:
     ```bash
     dotnet run --project SmartBusinessPro.WinForms/SmartBusinessPro.WinForms.csproj
     ```
   - **Consola**:
     ```bash
     dotnet run --project "coleccion productos smarbussines/coleccion productos smarbussines.csproj"
     ```
