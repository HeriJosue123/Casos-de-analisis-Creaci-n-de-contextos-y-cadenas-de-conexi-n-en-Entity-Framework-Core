# Práctica de Análisis: Creación de Contextos y Cadenas de Conexión en Entity Framework Core (SmartBusinessPro)

## 1. Nombre del Proyecto
SmartBusinessPro - Casos de análisis: Creación de contextos y cadenas de conexión en Entity Framework Core.

## 2. Objetivo de la Práctica
Esta práctica académica para la carrera de Ingeniería en Sistemas tiene como propósito implementar el acceso a datos mediante Entity Framework Core 10 sobre la plataforma .NET 10. La actividad consiste en:
- Crear y configurar una clase de contexto de base de datos (`SmartBusinessContext`) derivada de `DbContext`.
- Configurar la cadena de conexión de forma externa en el archivo `App.config` para enlazar con SQL Server.
- Mapear las entidades `Producto` y `Categoria` con sus respectivas relaciones.
- Consultar los productos mediante expresiones LINQ asíncronas y presentarlos en un control `DataGridView` dentro de una aplicación de Windows Forms.

## 3. Descripción Breve de la Solución
La solución está desarrollada en C# y .NET 10, y se compone de dos proyectos:
- **SmartBusinessPro.WinForms**: Proyecto principal de la práctica. Implementa la interfaz gráfica de usuario en Windows Forms, el contexto de Entity Framework Core y las consultas asíncronas para presentar el inventario.
- **coleccion productos smarbussines**: Proyecto complementario de consola que realiza una consulta equivalente a las mismas tablas utilizando ADO.NET clásico (`Microsoft.Data.SqlClient` y `SqlDataReader`). Se conserva en la solución como referencia comparativa entre ambos enfoques de acceso a datos.

## 4. Estructura de los Archivos Principales
```text
coleccion productos smarbussines.slnx
├── SmartBusinessPro.WinForms/
│   ├── Models/
│   │   ├── Categoria.cs            Entidad Categoria (relación uno a muchos)
│   │   ├── Producto.cs             Entidad Producto (clave foránea y navegación)
│   │   └── SmartBusinessContext.cs Contexto de base de datos derivado de DbContext
│   ├── App.config                  Configuración externa de la cadena de conexión
│   ├── Form1.cs                    Lógica de carga asíncrona y configuración de la tabla
│   ├── Form1.Designer.cs           Definición visual del formulario y DataGridView
│   ├── Form1.resx                  Recursos del formulario
│   ├── Program.cs                  Punto de entrada de la aplicación Windows Forms
│   └── SmartBusinessPro.WinForms.csproj
├── coleccion productos smarbussines/
│   ├── Program.cs                  Consulta a SQL Server con ADO.NET tradicional
│   └── coleccion productos smarbussines.csproj
├── .gitignore                      Exclusión de binarios, .vs y temporales
└── README.md                       Documentación de la práctica académica
```

## 5. Explicación de Producto y Categoria
Las entidades representan las tablas de la base de datos relacional:

- **Categoria (`Models/Categoria.cs`)**:
  - `CategoriaId` (int): Clave primaria de la categoría.
  - `Nombre` (string): Nombre asignado a la categoría.
  - `Productos` (`ICollection<Producto>`): Propiedad de navegación de colección que establece la relación uno a muchos hacia los productos vinculados.

- **Producto (`Models/Producto.cs`)**:
  - `ProductoId` (int): Clave primaria del producto.
  - `Nombre` (string): Nombre del artículo.
  - `Precio` (decimal): Precio unitario.
  - `Stock` (int): Existencias disponibles en el inventario.
  - `Activo` (bool): Indica si el producto está activo.
  - `CategoriaId` (int): Clave foránea que referencia a la categoría asociada.
  - `Categoria` (`Categoria?`): Propiedad de navegación de referencia hacia la entidad `Categoria`.

## 6. Explicación de SmartBusinessContext, DbSet y UseSqlServer
La clase `SmartBusinessContext` hereda de `DbContext` y centraliza la comunicación con la base de datos:

- **Propiedades DbSet**:
  - `public DbSet<Producto> Productos { get; set; }`
  - `public DbSet<Categoria> Categorias { get; set; }`
  Estas propiedades permiten consultar y trabajar con las entidades de la base de datos mediante Entity Framework Core utilizando expresiones LINQ.

- **Método OnConfiguring y UseSqlServer**:
  En el método `OnConfiguring`:
  1. Se verifica si el contexto no ha sido configurado previamente (`!optionsBuilder.IsConfigured`).
  2. Se obtiene la cadena de conexión de `App.config` mediante `ConfigurationManager`.
  3. Si la cadena no se encuentra o está vacía, se lanza una excepción `InvalidOperationException` informando el problema.
  4. Se invoca `optionsBuilder.UseSqlServer(...)` para registrar el proveedor de SQL Server.
  5. Se incorpora la opción `EnableRetryOnFailure(maxRetryCount: 3, maxRetryDelay: TimeSpan.FromSeconds(5))` para reintentar la conexión ante fallas transitorias de red o servicio.

## 7. Configuración de App.config
La cadena de conexión se encuentra definida en `SmartBusinessPro.WinForms/App.config`:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
	<connectionStrings>
		<add name="SmartBusinessContext"
			 connectionString="Server=THEyosua_503;Database=SmartBusinessPro.;Integrated Security=True;TrustServerCertificate=True;"
			 providerName="Microsoft.Data.SqlClient" />
	</connectionStrings>
</configuration>
```

Observaciones de configuración:
- **Nombre del servidor (`Server=THEyosua_503`)**: Corresponde al equipo e instancia local donde se ejecutó la práctica. Para abrir y probar la aplicación en otra computadora, se debe reemplazar este valor por el nombre del servidor o instancia correspondiente (por ejemplo: `.` o `localhost` o `.\SQLEXPRESS`).
- **Nombre de la base de datos (`Database=SmartBusinessPro.`)**: La base de datos fue creada con un punto al final (`SmartBusinessPro.`). Dicho nombre debe coincidir exactamente en la instancia local de SQL Server.
- **Seguridad integrada (`Integrated Security=True`)**: Utiliza la autenticación de Windows del usuario local, sin almacenar contraseñas ni credenciales en el archivo.

## 8. Consulta y Presentación de los Productos
La consulta se ejecuta al cargar la ventana en el evento `Form1_Load`:

1. Se crea una instancia de `SmartBusinessContext` dentro de un bloque `using` para liberar los recursos al finalizar.
2. Se ejecuta una consulta LINQ asíncrona utilizando `AsNoTracking()` para optimizar el rendimiento al tratarse de una operación de solo lectura.
3. Se seleccionan los campos requeridos mediante una proyección anónima:
   - `Producto`: nombre del artículo (`p.Nombre`).
   - `Precio`: valor monetario (`p.Precio`).
   - `Existencias`: cantidad disponible (`p.Stock`).
   - `Categoria`: nombre de la categoría vinculada, o `"Sin categoría"` en caso de ser nula.
4. La lista obtenida mediante `ToListAsync()` se enlaza a `dataGridView1.DataSource`.
5. Se aplica el formato `"C2"` a la columna `Precio` para mostrar el valor en formato de moneda.
6. La aplicación muestra correctamente los siete productos registrados en la base de datos.

## 9. Manejo de Errores y Pruebas Realizadas
- **Pruebas realizadas**:
  - Compilación de la solución completa en .NET 10 verificada con cero errores (0 errores).
  - Carga y visualización correcta de los siete registros de productos en el `DataGridView`.
- **Manejo de errores implementado**:
  - Si la cadena de conexión no existe en `App.config`, `SmartBusinessContext` genera una excepción `InvalidOperationException` detallada.
  - En `Form1.cs`, un bloque `try-catch` captura excepciones de conexión (como servicio detenido o nombre de servidor erróneo) y muestra un cuadro de diálogo descriptivo indicando al usuario que verifique el estado del servidor.
  - Si la base de datos responde pero no contiene registros (`productos.Count == 0`), se muestra un mensaje informativo indicando que no se encontraron productos.

## 10. Requisitos
- .NET SDK 10.0 instalado.
- Visual Studio 2022 (con compatibilidad para .NET 10 y herramientas de escritorio .NET).
- Microsoft SQL Server con la base de datos `SmartBusinessPro.` en ejecución.

## 11. Enlace al Repositorio
Repositorio en GitHub:
https://github.com/HeriJosue123/Casos-de-analisis-Creaci-n-de-contextos-y-cadenas-de-conexi-n-en-Entity-Framework-Core
