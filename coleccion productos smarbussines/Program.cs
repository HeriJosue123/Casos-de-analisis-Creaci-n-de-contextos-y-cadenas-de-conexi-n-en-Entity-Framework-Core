using System;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;

namespace Coleccion_Producto_SmartBusiness
{
    internal class ConsultaProducti
    {
        public int ProductoId { get; set; }

        public string Producto { get; set; }

        public string categoria { get; set; }

        public decimal Precio { get; set; }

        public int stock { get; set; }
    }

    internal class program
    {
        static void Main(string[] args)
        {
            string connString = "Server=THEyosua_503;Database=SmartBusinessPro.;Integrated Security=True;TrustServerCertificate=True;";

            string sql = @"
                SELECT 
                    p.ProductoId,
                    p.Nombre AS Productos,
                    c.Nombre AS Categoria,
                    p.Precio,
                    p.Stock
                FROM Productos AS p
                INNER JOIN Categorias AS c 
                    ON p.CategoriaId = c.CategoriaId
                WHERE p.Activo = 1
                ORDER BY p.Precio DESC;";

            List<ConsultaProducti> productos = new List<ConsultaProducti>();

            using (SqlConnection connection = new SqlConnection(connString))
            {
                SqlCommand command = new SqlCommand(sql, connection);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ConsultaProducti producto = new ConsultaProducti
                    {
                        ProductoId = reader.GetInt32(0),
                        Producto = reader.GetString(1),
                        categoria = reader.GetString(2),
                        Precio = reader.GetDecimal(3),
                        stock = reader.GetInt32(4)
                    };

                    productos.Add(producto);
                }

                reader.Close();
            }

            foreach (var producto in productos)
            {
                Console.WriteLine(
                    $"ID: {producto.ProductoId}, " +
                    $"Producto: {producto.Producto}, " +
                    $"Categoria: {producto.categoria}, " +
                    $"Precio: {producto.Precio}, " +
                    $"Stock: {producto.stock}"
                );
            }

            Console.WriteLine("\nPresione cualquier tecla para salir...");
            Console.ReadKey();
        }
    }
}