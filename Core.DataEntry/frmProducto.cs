using Core.Models;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Core.DataEntry
{
    public partial class frmProducto : Form
    {
        // Variable para saber qué producto estamos editando o eliminando
        private int idProductoSeleccionado = 0;

        public frmProducto()
        {
            InitializeComponent();
        }

        private void frmProducto_Load(object sender, EventArgs e)
        {
            CargarProductos(); // Cargar la tabla automáticamente al abrir el formulario
        }

        // --- READ: Leer datos de la BD ---
        private void CargarProductos()
        {
            try
            {
                using (var db = new GomasContext())
                {
                    // Traemos todos los productos activos
                    var query = db.Productos
                                  .Where(p => p.Estado == true)
                                  .Select(p => new
                                  {
                                      p.IdProducto,
                                      p.Marca,
                                      p.Modelo,
                                      p.Medida,
                                      p.Costo,
                                      p.PrecioVenta
                                  }).ToList();

                    dgvProductos.DataSource = query;

                    // Ocultar la columna del ID para que el usuario no la vea
                    if (dgvProductos.Columns["IdProducto"] != null)
                        dgvProductos.Columns["IdProducto"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los productos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- CREATE: Insertar ---
        private void btnInsertarProducto_Click(object sender, EventArgs e)
        {
            // Evitar que el usuario intente crear un registro nuevo mientras tiene uno seleccionado
            if (idProductoSeleccionado != 0)
            {
                MessageBox.Show("Actualmente tiene un producto seleccionado. Para agregar uno nuevo, presione Limpiar primero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Validar textos
            if (string.IsNullOrWhiteSpace(txtMarca.Text) || string.IsNullOrWhiteSpace(txtModelo.Text) || string.IsNullOrWhiteSpace(txtMedida.Text))
            {
                MessageBox.Show("Los campos Marca, Modelo y Medida son obligatorios.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validar lógica de negocio (Números)
            if (nudPrecioVenta.Value <= 0 || nudCosto.Value <= 0)
            {
                MessageBox.Show("El precio de venta y el costo deben ser mayores a cero.", "Valores inválidos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var db = new GomasContext())
                {
                    var nuevoProducto = new Producto
                    {
                        Marca = txtMarca.Text,
                        Modelo = txtModelo.Text,
                        Medida = txtMedida.Text,
                        Costo = nudCosto.Value,
                        PrecioVenta = nudPrecioVenta.Value,
                        Estado = true
                    };

                    db.Productos.Add(nuevoProducto);
                    db.SaveChanges();

                    MessageBox.Show("Producto registrado exitosamente en el inventario.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarCampos();
                    CargarProductos(); // Refrescar la tabla
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar en la base de datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- UPDATE: Editar ---
        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (idProductoSeleccionado == 0)
            {
                MessageBox.Show("Por favor, seleccione un producto de la tabla para editarlo.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtMarca.Text) || string.IsNullOrWhiteSpace(txtModelo.Text) || string.IsNullOrWhiteSpace(txtMedida.Text))
            {
                MessageBox.Show("Los campos Marca, Modelo y Medida no pueden estar vacíos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var db = new GomasContext())
                {
                    var productoObj = db.Productos.Find(idProductoSeleccionado);

                    if (productoObj != null)
                    {
                        productoObj.Marca = txtMarca.Text;
                        productoObj.Modelo = txtModelo.Text;
                        productoObj.Medida = txtMedida.Text;
                        productoObj.Costo = nudCosto.Value;
                        productoObj.PrecioVenta = nudPrecioVenta.Value;

                        db.SaveChanges();
                        MessageBox.Show("Producto actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LimpiarCampos();
                        CargarProductos();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- DELETE (Soft Delete): Eliminar ---
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (idProductoSeleccionado == 0)
            {
                MessageBox.Show("Por favor, seleccione un producto de la tabla para eliminarlo.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult dialogo = MessageBox.Show("¿Está seguro que desea dar de baja este producto del inventario?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dialogo == DialogResult.Yes)
            {
                try
                {
                    using (var db = new GomasContext())
                    {
                        var productoObj = db.Productos.Find(idProductoSeleccionado);
                        if (productoObj != null)
                        {
                            productoObj.Estado = false; // Eliminación Lógica
                            db.SaveChanges();

                            MessageBox.Show("Producto eliminado del sistema exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LimpiarCampos();
                            CargarProductos();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // --- SELECCIONAR (CellClick) ---
        private void dgvProductos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dgvProductos.Rows[e.RowIndex];

                idProductoSeleccionado = Convert.ToInt32(fila.Cells["IdProducto"].Value);

                txtMarca.Text = fila.Cells["Marca"].Value?.ToString();
                txtModelo.Text = fila.Cells["Modelo"].Value?.ToString();
                txtMedida.Text = fila.Cells["Medida"].Value?.ToString();

                if (fila.Cells["Costo"].Value != null)
                    nudCosto.Value = Convert.ToDecimal(fila.Cells["Costo"].Value);

                if (fila.Cells["PrecioVenta"].Value != null)
                    nudPrecioVenta.Value = Convert.ToDecimal(fila.Cells["PrecioVenta"].Value);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void LimpiarCampos()
        {
            idProductoSeleccionado = 0; // Soltamos el ID
            txtMarca.Clear();
            txtModelo.Clear();
            txtMedida.Clear();

            // Regresamos los NumericUpDown a su mínimo permitido (usualmente 0)
            nudCosto.Value = nudCosto.Minimum;
            nudPrecioVenta.Value = nudPrecioVenta.Minimum;

            dgvProductos.ClearSelection();
            txtMarca.Focus();
        }
    }
}