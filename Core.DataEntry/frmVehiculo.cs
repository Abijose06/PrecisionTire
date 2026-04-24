using Core.Models;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Core.DataEntry
{
    public partial class frmVehiculo : Form
    {
        // Variable global para rastrear el vehículo seleccionado
        private int idVehiculoSeleccionado = 0;

        public frmVehiculo()
        {
            InitializeComponent();
        }

        private void frmVehiculo_Load(object sender, EventArgs e)
        {
            CargarClientes(); // Primero cargamos el combobox para que esté listo
            CargarVehiculos(); // Luego cargamos la tabla
        }

        // --- PREPARAR INTERFAZ: Cargar Clientes en el ComboBox ---
        private void CargarClientes()
        {
            try
            {
                using (var db = new GomasContext())
                {
                    // Buscamos solo a los usuarios activos que tienen el rol de "Cliente"
                    var clientes = db.Usuarios
                                     .Where(u => u.Rol == "Cliente" && u.Estado == true)
                                     .Select(u => new
                                     {
                                         u.IdUsuario,
                                         // Concatenamos Nombres y Apellidos para que se vea más profesional
                                         NombreCompleto = u.Nombres + " " + u.Apellidos
                                     }).ToList();

                    cmbCliente.DataSource = clientes;
                    cmbCliente.DisplayMember = "NombreCompleto"; // Lo que ve el usuario
                    cmbCliente.ValueMember = "IdUsuario";        // El ID real que guardaremos
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los clientes: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // --- READ: Leer datos de la BD ---
        private void CargarVehiculos()
        {
            try
            {
                using (var db = new GomasContext())
                {
                    // Hacemos un JOIN entre Vehiculos y Usuarios para traer el nombre del dueño
                    var query = from v in db.Vehiculos
                                join u in db.Usuarios on v.IdCliente equals u.IdUsuario
                                where v.Estado == true
                                select new
                                {
                                    IdVehiculo = v.IdVehiculo,
                                    IdCliente = v.IdCliente,
                                    ClienteDueño = u.Nombres + " " + u.Apellidos,
                                    Marca = v.Marca,
                                    Modelo = v.Modelo,
                                    Año = v.Año,
                                    Placa = v.Placa,
                                    Chasis = v.Chasis
                                };

                    dgvVehiculos.DataSource = query.ToList();

                    // Ocultamos las columnas de IDs
                    if (dgvVehiculos.Columns["IdVehiculo"] != null)
                        dgvVehiculos.Columns["IdVehiculo"].Visible = false;

                    if (dgvVehiculos.Columns["IdCliente"] != null)
                        dgvVehiculos.Columns["IdCliente"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                string errorReal = ex.Message;

                // Escarbamos hasta llegar al error original de SQL Server
                if (ex.InnerException != null)
                {
                    errorReal += "\nDetalle interno: " + ex.InnerException.Message;
                    if (ex.InnerException.InnerException != null)
                    {
                        errorReal += "\nError de SQL: " + ex.InnerException.InnerException.Message;
                    }
                }

                MessageBox.Show("Error al cargar los vehículos:\n\n" + errorReal, "Error Forense", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- CREATE: Insertar ---
        private void btnInsertarVehiculo_Click(object sender, EventArgs e)
        {
            if (idVehiculoSeleccionado != 0)
            {
                MessageBox.Show("Actualmente tiene un vehículo seleccionado. Para agregar uno nuevo, presione Limpiar primero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Validaciones
            if (string.IsNullOrWhiteSpace(txtPlaca.Text) || cmbCliente.SelectedValue == null)
            {
                MessageBox.Show("La placa y el cliente (dueño) son obligatorios.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var db = new GomasContext())
                {
                    var nuevoVehiculo = new Vehiculo
                    {
                        IdCliente = Convert.ToInt32(cmbCliente.SelectedValue),
                        Marca = txtMarca.Text,
                        Modelo = txtModelo.Text,
                        Año = Convert.ToInt16(nudAño.Value),
                        Placa = txtPlaca.Text,
                        Chasis = txtChassis.Text,
                        Estado = true
                    };

                    db.Vehiculos.Add(nuevoVehiculo);
                    db.SaveChanges();

                    MessageBox.Show("Vehículo registrado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarCampos();
                    CargarVehiculos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar vehículo: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- UPDATE: Editar ---
        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (idVehiculoSeleccionado == 0)
            {
                MessageBox.Show("Por favor, seleccione un vehículo de la tabla para editarlo.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPlaca.Text) || cmbCliente.SelectedValue == null)
            {
                MessageBox.Show("La placa y el cliente son obligatorios.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var db = new GomasContext())
                {
                    var vehiculoObj = db.Vehiculos.Find(idVehiculoSeleccionado);

                    if (vehiculoObj != null)
                    {
                        vehiculoObj.IdCliente = Convert.ToInt32(cmbCliente.SelectedValue);
                        vehiculoObj.Marca = txtMarca.Text;
                        vehiculoObj.Modelo = txtModelo.Text;
                        vehiculoObj.Año = Convert.ToInt16(nudAño.Value);
                        vehiculoObj.Placa = txtPlaca.Text;
                        vehiculoObj.Chasis = txtChassis.Text;

                        db.SaveChanges();
                        MessageBox.Show("Datos del vehículo actualizados correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LimpiarCampos();
                        CargarVehiculos();
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
            if (idVehiculoSeleccionado == 0)
            {
                MessageBox.Show("Por favor, seleccione un vehículo de la tabla para eliminarlo.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult dialogo = MessageBox.Show("¿Está seguro que desea dar de baja este vehículo del sistema?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dialogo == DialogResult.Yes)
            {
                try
                {
                    using (var db = new GomasContext())
                    {
                        var vehiculoObj = db.Vehiculos.Find(idVehiculoSeleccionado);
                        if (vehiculoObj != null)
                        {
                            vehiculoObj.Estado = false; // Eliminación Lógica
                            db.SaveChanges();

                            MessageBox.Show("Vehículo eliminado del sistema exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LimpiarCampos();
                            CargarVehiculos();
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
        private void dgvVehiculos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dgvVehiculos.Rows[e.RowIndex];

                idVehiculoSeleccionado = Convert.ToInt32(fila.Cells["IdVehiculo"].Value);

                // Cargar el Combobox de Cliente
                if (fila.Cells["IdCliente"].Value != null)
                    cmbCliente.SelectedValue = Convert.ToInt32(fila.Cells["IdCliente"].Value);

                txtMarca.Text = fila.Cells["Marca"].Value?.ToString();
                txtModelo.Text = fila.Cells["Modelo"].Value?.ToString();
                txtPlaca.Text = fila.Cells["Placa"].Value?.ToString();
                txtChassis.Text = fila.Cells["Chasis"].Value?.ToString();

                if (fila.Cells["Año"].Value != null)
                    nudAño.Value = Convert.ToDecimal(fila.Cells["Año"].Value);
            }
        }

        // --- LIMPIAR CAMPOS ---
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void LimpiarCampos()
        {
            idVehiculoSeleccionado = 0;
            txtMarca.Clear();
            txtModelo.Clear();
            txtPlaca.Clear();
            txtChassis.Clear();

            // Regresamos el año al actual
            nudAño.Value = DateTime.Now.Year;

            // Seleccionamos el primer cliente de la lista si hay alguno
            if (cmbCliente.Items.Count > 0)
                cmbCliente.SelectedIndex = 0;

            dgvVehiculos.ClearSelection();
            txtMarca.Focus();
        }
    }
}