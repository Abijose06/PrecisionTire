using Core.Models;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Core.DataEntry
{
    public partial class frmSucursal : Form
    {
        // Variable para saber qué sucursal estamos editando o eliminando
        private int idSucursalSeleccionada = 0;

        public frmSucursal()
        {
            InitializeComponent();
        }

        private void frmSucursal_Load(object sender, EventArgs e)
        {
            CargarSucursales(); // Cargar la tabla automáticamente al abrir el formulario
        }

        // --- READ: Leer datos de la BD ---
        private void CargarSucursales()
        {
            try
            {
                using (var db = new GomasContext())
                {
                    // Traemos todas las sucursales activas
                    var query = db.Sucursales
                                  .Where(s => s.Estado == true)
                                  .Select(s => new
                                  {
                                      s.IdSucursal,
                                      s.Direccion,
                                      s.Telefono
                                  }).ToList();

                    dgvSucursales.DataSource = query;

                    // Ocultar la columna del ID para que el usuario no la vea
                    if (dgvSucursales.Columns["IdSucursal"] != null)
                        dgvSucursales.Columns["IdSucursal"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar las sucursales: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- CREATE: Insertar ---
        private void btnInsertarSucursal_Click(object sender, EventArgs e)
        {
            // Evitar que el usuario intente crear un registro nuevo mientras tiene uno seleccionado
            if (idSucursalSeleccionada != 0)
            {
                MessageBox.Show("Actualmente tiene una sucursal seleccionada. Para agregar una nueva, presione Limpiar primero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Validar textos
            if (string.IsNullOrWhiteSpace(txtDireccion.Text) || string.IsNullOrWhiteSpace(txtTelefono.Text))
            {
                MessageBox.Show("Los campos Dirección y Teléfono son obligatorios.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var db = new GomasContext())
                {
                    var nuevaSucursal = new Sucursal
                    {
                        Direccion = txtDireccion.Text,
                        Telefono = txtTelefono.Text,
                        Estado = true
                    };

                    db.Sucursales.Add(nuevaSucursal);
                    db.SaveChanges();

                    MessageBox.Show("Sucursal registrada exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarCampos();
                    CargarSucursales(); // Refrescar la tabla
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
            if (idSucursalSeleccionada == 0)
            {
                MessageBox.Show("Por favor, seleccione una sucursal de la tabla para editarla.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDireccion.Text) || string.IsNullOrWhiteSpace(txtTelefono.Text))
            {
                MessageBox.Show("Los campos Dirección y Teléfono no pueden estar vacíos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var db = new GomasContext())
                {
                    var sucursalObj = db.Sucursales.Find(idSucursalSeleccionada);

                    if (sucursalObj != null)
                    {
                        sucursalObj.Direccion = txtDireccion.Text;
                        sucursalObj.Telefono = txtTelefono.Text;

                        db.SaveChanges();
                        MessageBox.Show("Sucursal actualizada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LimpiarCampos();
                        CargarSucursales();
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
            if (idSucursalSeleccionada == 0)
            {
                MessageBox.Show("Por favor, seleccione una sucursal de la tabla para eliminarla.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult dialogo = MessageBox.Show("¿Está seguro que desea dar de baja a esta sucursal del sistema?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dialogo == DialogResult.Yes)
            {
                try
                {
                    using (var db = new GomasContext())
                    {
                        var sucursalObj = db.Sucursales.Find(idSucursalSeleccionada);
                        if (sucursalObj != null)
                        {
                            sucursalObj.Estado = false; // Eliminación Lógica
                            db.SaveChanges();

                            MessageBox.Show("Sucursal eliminada del sistema exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LimpiarCampos();
                            CargarSucursales();
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
        private void dgvSucursales_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dgvSucursales.Rows[e.RowIndex];

                idSucursalSeleccionada = Convert.ToInt32(fila.Cells["IdSucursal"].Value);

                txtDireccion.Text = fila.Cells["Direccion"].Value?.ToString();
                txtTelefono.Text = fila.Cells["Telefono"].Value?.ToString();
            }
        }

        // --- LIMPIAR CAMPOS ---
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void LimpiarCampos()
        {
            idSucursalSeleccionada = 0; // Soltamos el ID
            txtDireccion.Clear();
            txtTelefono.Clear();

            dgvSucursales.ClearSelection();
            txtDireccion.Focus();
        }
    }
}