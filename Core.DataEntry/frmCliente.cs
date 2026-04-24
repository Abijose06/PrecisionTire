using Core.Helpers;
using Core.Models;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Core.DataEntry
{
    public partial class frmCliente : Form
    {
        // Variable para saber qué cliente estamos editando o eliminando
        private int idUsuarioSeleccionado = 0;

        public frmCliente()
        {
            InitializeComponent();
        }

        private void frmCliente_Load(object sender, EventArgs e)
        {
            CargarClientes(); // Cargar la tabla al abrir el formulario
        }

        // --- READ: Leer datos de la BD ---
        private void CargarClientes()
        {
            try
            {
                using (var db = new GomasContext())
                {
                    // Hacemos un JOIN entre Usuarios y Clientes para traer todo junto
                    var query = from u in db.Usuarios
                                join c in db.Clientes on u.IdUsuario equals c.IdUsuario
                                where u.Rol == "Cliente" && u.Estado == true // Solo los activos
                                select new
                                {
                                    IdUsuario = u.IdUsuario,
                                    TipoDocumento = u.TipoDocumento,
                                    Documento = u.Documento,
                                    Nombres = u.Nombres,
                                    Apellidos = u.Apellidos,
                                    Telefono = u.Telefono,
                                    Correo = u.Correo,
                                    Direccion = c.Direccion
                                };

                    dgvClientes.DataSource = query.ToList();

                    // Ocultar la columna del ID para que el usuario no la vea
                    if (dgvClientes.Columns["IdUsuario"] != null)
                        dgvClientes.Columns["IdUsuario"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los clientes: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- CREATE: Insertar ---
        private void btnInsertarCliente_Click(object sender, EventArgs e)
        {
            if (idUsuarioSeleccionado != 0)
            {
                MessageBox.Show("Actualmente tiene un cliente seleccionado. Para agregar uno nuevo, presione Limpiar primero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDocumento.Text) || string.IsNullOrWhiteSpace(txtContraseña.Text) || string.IsNullOrWhiteSpace(txtNombres.Text))
            {
                MessageBox.Show("Por favor llene los campos obligatorios.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var db = new GomasContext())
                {
                    using (var transaccion = db.Database.BeginTransaction())
                    {
                        try
                        {
                            var nuevoUsuario = new Usuario
                            {
                                TipoDocumento = Convert.ToInt32(txtTipoDocumento.Text),
                                Documento = txtDocumento.Text,
                                ClaveHash = SeguridadHelper.CalcularHash(txtContraseña.Text),
                                Rol = "Cliente",
                                Nombres = txtNombres.Text,
                                Apellidos = txtApellidos.Text,
                                Telefono = txtTelefono.Text,
                                Correo = txtCorreo.Text,
                                Estado = true
                            };

                            db.Usuarios.Add(nuevoUsuario);
                            db.SaveChanges();

                            var nuevoCliente = new Cliente
                            {
                                IdUsuario = nuevoUsuario.IdUsuario,
                                Direccion = txtDireccion.Text
                            };

                            db.Clientes.Add(nuevoCliente);
                            db.SaveChanges();

                            transaccion.Commit();
                            MessageBox.Show("Cliente registrado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LimpiarCampos();
                            CargarClientes(); // Refrescar la tabla
                        }
                        catch (Exception ex)
                        {
                            transaccion.Rollback();
                            MessageBox.Show("Error al guardar en la base de datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Verifique los datos ingresados. Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- UPDATE: Editar ---
        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (idUsuarioSeleccionado == 0)
            {
                MessageBox.Show("Por favor, seleccione un cliente de la tabla para editarlo.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var db = new GomasContext())
                {
                    // Buscamos el usuario y su registro de cliente
                    var usuarioObj = db.Usuarios.Find(idUsuarioSeleccionado);
                    var clienteObj = db.Clientes.FirstOrDefault(c => c.IdUsuario == idUsuarioSeleccionado);

                    if (usuarioObj != null && clienteObj != null)
                    {
                        usuarioObj.TipoDocumento = Convert.ToInt32(txtTipoDocumento.Text);
                        usuarioObj.Documento = txtDocumento.Text;
                        usuarioObj.Nombres = txtNombres.Text;
                        usuarioObj.Apellidos = txtApellidos.Text;
                        usuarioObj.Telefono = txtTelefono.Text;
                        usuarioObj.Correo = txtCorreo.Text;
                        clienteObj.Direccion = txtDireccion.Text;

                        // Solo cambiamos la contraseña si escribió algo nuevo
                        if (!string.IsNullOrWhiteSpace(txtContraseña.Text))
                        {
                            usuarioObj.ClaveHash = SeguridadHelper.CalcularHash(txtContraseña.Text);
                        }

                        db.SaveChanges();
                        MessageBox.Show("Cliente actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LimpiarCampos();
                        CargarClientes();
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
            if (idUsuarioSeleccionado == 0)
            {
                MessageBox.Show("Por favor, seleccione un cliente de la tabla para eliminarlo.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult dialogo = MessageBox.Show("¿Está seguro que desea eliminar este cliente?", "Confirmar Eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dialogo == DialogResult.Yes)
            {
                try
                {
                    using (var db = new GomasContext())
                    {
                        var usuarioObj = db.Usuarios.Find(idUsuarioSeleccionado);
                        if (usuarioObj != null)
                        {
                            usuarioObj.Estado = false; // Eliminación lógica
                            db.SaveChanges();

                            MessageBox.Show("Cliente eliminado del sistema.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LimpiarCampos();
                            CargarClientes();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // --- SELECCIONAR de la tabla ---
        private void dgvClientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Verificamos que no haya hecho clic en los encabezados
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dgvClientes.Rows[e.RowIndex];

                idUsuarioSeleccionado = Convert.ToInt32(fila.Cells["IdUsuario"].Value);
                txtTipoDocumento.Text = fila.Cells["TipoDocumento"].Value?.ToString();
                txtDocumento.Text = fila.Cells["Documento"].Value?.ToString();
                txtNombres.Text = fila.Cells["Nombres"].Value?.ToString();
                txtApellidos.Text = fila.Cells["Apellidos"].Value?.ToString();
                txtTelefono.Text = fila.Cells["Telefono"].Value?.ToString();
                txtCorreo.Text = fila.Cells["Correo"].Value?.ToString();
                txtDireccion.Text = fila.Cells["Direccion"].Value?.ToString();

                txtContraseña.Clear(); // No cargamos el hash por seguridad
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void LimpiarCampos()
        {
            idUsuarioSeleccionado = 0; // Reseteamos la selección
            txtTipoDocumento.Clear();
            txtDocumento.Clear();
            txtContraseña.Clear();
            txtNombres.Clear();
            txtApellidos.Clear();
            txtTelefono.Clear();
            txtCorreo.Clear();
            txtDireccion.Clear();
            dgvClientes.ClearSelection();
            txtTipoDocumento.Focus();
        }
    }
}