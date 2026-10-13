namespace SmartBusinessPro.WinForms.Models
{
    public class Producto
    {
        public int ProductoId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public bool Activo { get; set; }
        public int CategoriaId { get; set; }

        public Categoria? Categoria { get; set; }
    }
}