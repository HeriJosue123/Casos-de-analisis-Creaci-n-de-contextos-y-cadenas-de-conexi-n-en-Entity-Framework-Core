using Microsoft.EntityFrameworkCore;
using SmartBusinessPro.WinForms.Models;
using System;
using System.Linq;
using System.Windows.Forms;

namespace SmartBusinessPro.WinForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Text = "SmartBusinessPro - Inventario";
            WindowState = FormWindowState.Maximized;
            StartPosition = FormStartPosition.CenterScreen;

            // Configurar la tabla existente en el diseñador
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.ReadOnly = true;
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.RowHeadersVisible = false;

            // Cargar los productos cuando se abra la ventana
            Load += Form1_Load;
        }

        private async void Form1_Load(object? sender, EventArgs e)
        {
            try
            {
                using var contexto = new SmartBusinessContext(
                    new DbContextOptionsBuilder<SmartBusinessContext>().Options);

                var productos = await contexto.Productos
                    .AsNoTracking()
                    .Select(p => new
                    {
                        Producto = p.Nombre,
                        Precio = p.Precio,
                        Existencias = p.Stock,
                        Categoria = p.Categoria != null
                            ? p.Categoria.Nombre
                            : "Sin categoría"
                    })
                    .ToListAsync();

                dataGridView1.DataSource = productos;

                if (dataGridView1.Columns["Precio"] != null)
                {
                    dataGridView1.Columns["Precio"]!
                        .DefaultCellStyle.Format = "C2";
                }

                if (productos.Count == 0)
                {
                    MessageBox.Show(
                        "No se encontraron productos en la base de datos.",
                        "Inventario",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudieron cargar los productos.\n\n" +
                    "Verificá que SQL Server esté iniciado y que la cadena " +
                    "de conexión sea correcta.\n\nDetalle:\n" + ex.Message,
                    "Error de conexión",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}