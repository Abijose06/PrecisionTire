using Core.Models;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Core.DataEntry
{
    public partial class frmServicio : Form
    {
        // Variable para saber qué servicio estamos editando o eliminando
        private int idServicioSeleccionado = 0;

        public frmServicio()
        {
            InitializeComponent();
        }

        private void frmServicio_Load(object sender, EventArgs e)
        {
            CargarServicios(); // Cargamos la tabla automáticamente al abrir
        }

        // --- READ: Leer datos de la BD ---
        private void CargarServicios()
        {
            try
            {
                using (var db = new GomasContext())
                {
                    // Traemos todos los servicios que estén activos
                    var query = db.Servicios
                                  .Where(s => s.Estado == true)
                                  .Select(s => new
                                  {
                                      s.IdServicio,
                                      s.NombreServicio,
                                      s.Descripcion,
                                      s.Precio
                                  }).ToList();

                    dgvServicios.DataSource = query;

                    // Ocultamos la columna del ID para no confundir al usuario
                    if (dgvServicios.Columns["IdServicio"] != null)
                        dgvServicios.Columns["IdServicio"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los servicios: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- CREATE: Insertar ---
        private void btnInsertarServicio_Click(object sender, EventArgs e)
        {
            // Evitar que cree uno nuevo si está editando
            if (idServicioSeleccionado != 0)
            {
                MessageBox.Show("Actualmente tiene un servicio seleccionado. Para agregar uno nuevo, presione Limpiar primero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Validaciones
            if (string.IsNullOrWhiteSpace(txtNombreServicio.Text))
            {
                MessageBox.Show("El nombre del servicio es obligatorio.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (nudPrecio.Value <= 0)
            {
                MessageBox.Show("El precio del servicio debe ser mayor a cero.", "Valores inválidos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var db = new GomasContext())
                {
                    var nuevoServicio = new Servicio
                    {
                        NombreServicio = txtNombreServicio.Text,
                        Descripcion = txtDescripcion.Text,
                        Precio = nudPrecio.Value,
                        Estado = true
                    };

                    db.Servicios.Add(nuevoServicio);
                    db.SaveChanges();

                    MessageBox.Show("Servicio registrado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarCampos();
                    CargarServicios(); // Refrescamos la tabla
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
            if (idServicioSeleccionado == 0)
            {
                MessageBox.Show("Por favor, seleccione un servicio de la tabla para editarlo.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNombreServicio.Text))
            {
                MessageBox.Show("El nombre del servicio es obligatorio.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var db = new GomasContext())
                {
                    var servicioObj = db.Servicios.Find(idServicioSeleccionado);

                    if (servicioObj != null)
                    {
                        servicioObj.NombreServicio = txtNombreServicio.Text;
                        servicioObj.Descripcion = txtDescripcion.Text;
                        servicioObj.Precio = nudPrecio.Value;

                        db.SaveChanges();
                        MessageBox.Show("Servicio actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LimpiarCampos();
                        CargarServicios();
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
            if (idServicioSeleccionado == 0)
            {
                MessageBox.Show("Por favor, seleccione un servicio de la tabla para eliminarlo.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult dialogo = MessageBox.Show("¿Está seguro que desea dar de baja este servicio?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dialogo == DialogResult.Yes)
            {
                try
                {
                    using (var db = new GomasContext())
                    {
                        var servicioObj = db.Servicios.Find(idServicioSeleccionado);
                        if (servicioObj != null)
                        {
                            servicioObj.Estado = false; // Eliminación Lógica
                            db.SaveChanges();

                            MessageBox.Show("Servicio eliminado del sistema exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LimpiarCampos();
                            CargarServicios();
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
        private void dgvServicios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Validamos que se haya hecho clic en una fila válida y no en los encabezados
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dgvServicios.Rows[e.RowIndex];

                idServicioSeleccionado = Convert.ToInt32(fila.Cells["IdServicio"].Value);

                txtNombreServicio.Text = fila.Cells["NombreServicio"].Value?.ToString();
                txtDescripcion.Text = fila.Cells["Descripcion"].Value?.ToString();

                if (fila.Cells["Precio"].Value != null)
                    nudPrecio.Value = Convert.ToDecimal(fila.Cells["Precio"].Value);
            }
        }

        // --- LIMPIAR CAMPOS ---
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void LimpiarCampos()
        {
            idServicioSeleccionado = 0; // Soltamos el ID seleccionado
            txtNombreServicio.Clear();
            txtDescripcion.Clear();

            // Regresamos el precio a su mínimo permitido (0)
            nudPrecio.Value = nudPrecio.Minimum;

            dgvServicios.ClearSelection();
            txtNombreServicio.Focus();
        }
    }
}