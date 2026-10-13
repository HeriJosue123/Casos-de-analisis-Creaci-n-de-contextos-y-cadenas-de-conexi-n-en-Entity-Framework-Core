using Microsoft.EntityFrameworkCore;
using System;
using System.Configuration;

namespace SmartBusinessPro.WinForms.Models
{
    public class SmartBusinessContext : DbContext
    {
        public SmartBusinessContext(
            DbContextOptions<SmartBusinessContext> options)
            : base(options)
        {
        }

        public DbSet<Producto> Productos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }

        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string? conexion = ConfigurationManager
                    .ConnectionStrings["SmartBusinessContext"]?
                    .ConnectionString;

                if (string.IsNullOrWhiteSpace(conexion))
                {
                    throw new InvalidOperationException(
                        "No se encontró la cadena de conexión " +
                        "'SmartBusinessContext' en App.config.");
                }

                optionsBuilder.UseSqlServer(
                    conexion,
                    sql => sql.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromSeconds(5),
                        errorNumbersToAdd: null));
            }
        }
    }
}