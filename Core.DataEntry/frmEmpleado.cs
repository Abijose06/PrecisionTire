using Core.Helpers;
using Core.Models;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Core.DataEntry
{
    public partial class frmEmpleado : Form
    {

        // Variable global para rastrear la fila seleccionada
        private int idUsuarioSeleccionado = 0;


        public frmEmpleado()
        {
            InitializeComponent();
        }

        private void frmEmpleado_Load(object sender, EventArgs e)
        {
            CargarCombos();
            CargarEmpleados();
        }

        // --- PREPARAR INTERFAZ: Llenar ComboBoxes ---
        private void CargarCombos()
        {
            // 1. Llenar el ComboBox de Roles
            cmbRol.Items.Clear();
            cmbRol.Items.Add("Administrador");
            cmbRol.Items.Add("Cajero");
            cmbRol.Items.Add("Supervisor");
            cmbRol.Items.Add("Vendedor");
            cmbRol.SelectedIndex = 0; // Seleccionar el primero por defecto

            // 2. Llenar el ComboBox de Sucursales desde la BD
            try
            {
                using (var db = new GomasContext())
                {
                    var sucursales = db.Sucursales.Where(s => s.Estado == true).ToList();

                    // Añadimos una opción en blanco/nula en caso de que no tenga sucursal asignada
                    sucursales.Insert(0, new Sucursal { IdSucursal = 0, Direccion = "--- Sin Asignar ---" });

                    cmbSucursal.DataSource = sucursales;
                    cmbSucursal.DisplayMember = "Direccion"; // Lo que el usuario ve
                    cmbSucursal.ValueMember = "IdSucursal";  // El ID oculto
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar las sucursales: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // --- READ: Leer datos de la BD ---
        private void CargarEmpleados()
        {
            try
            {
                using (var db = new GomasContext())
                {
                    // Hacemos un JOIN múltiple: Usuarios + Empleados + Sucursales (Left Join)
                    var query = from u in db.Usuarios
                                join e in db.Empleados on u.IdUsuario equals e.IdUsuario
                                // Hacemos un Left Join con sucursales por si el empleado tiene IdSucursal nulo
                                join s in db.Sucursales on e.IdSucursal equals s.IdSucursal into sucursales
                                from s in sucursales.DefaultIfEmpty()
                                where u.Rol != "Cliente" && u.Estado == true // Solo empleados activos
                                select new
                                {
                                    IdUsuario = u.IdUsuario,
                                    TipoDocumento = u.TipoDocumento,
                                    Documento = u.Documento,
                                    Nombres = u.Nombres,
                                    Apellidos = u.Apellidos,
                                    Telefono = u.Telefono,
                                    Correo = u.Correo,
                                    Rol = u.Rol,
                                    Sueldo = e.Sueldo,
                                    FechaIngreso = e.FechaIngreso,
                                    IdSucursal = e.IdSucursal,
                                    Sucursal = s != null ? s.Direccion : "Sin Asignar"
                                };

                    dgvEmpleados.DataSource = query.ToList();

                    // Ocultamos las columnas técnicas que el usuario no necesita ver
                    if (dgvEmpleados.Columns["IdUsuario"] != null)
                        dgvEmpleados.Columns["IdUsuario"].Visible = false;

                    if (dgvEmpleados.Columns["IdSucursal"] != null)
                        dgvEmpleados.Columns["IdSucursal"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los empleados: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- CREATE: Insertar ---
        private void btnInsertarEmpleado_Click(object sender, EventArgs e)
        {
            if (idUsuarioSeleccionado != 0)
            {
                MessageBox.Show("Actualmente tiene un empleado seleccionado. Para agregar uno nuevo, presione Limpiar primero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtDocumento.Text) || string.IsNullOrWhiteSpace(txtContraseña.Text) || string.IsNullOrWhiteSpace(txtNombres.Text))
            {
                MessageBox.Show("Los campos Documento, Nombres y Contraseña son obligatorios.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var db = new GomasContext())
                {
                    // Iniciamos transacción por seguridad
                    using (var transaccion = db.Database.BeginTransaction())
                    {
                        try
                        {
                            // 1. Guardar en tblUsuario
                            var nuevoUsuario = new Usuario
                            {
                                TipoDocumento = string.IsNullOrWhiteSpace(txtTipoDocumento.Text) ? 0 : Convert.ToInt32(txtTipoDocumento.Text),
                                Documento = txtDocumento.Text,
                                ClaveHash = SeguridadHelper.CalcularHash(txtContraseña.Text),
                                Rol = cmbRol.SelectedItem.ToString(),
                                Nombres = txtNombres.Text,
                                Apellidos = txtApellidos.Text,
                                Telefono = txtTelefono.Text,
                                Correo = txtCorreo.Text,
                                Estado = true
                            };

                            db.Usuarios.Add(nuevoUsuario);
                            db.SaveChanges(); // Genera el IdUsuario

                            // 2. Guardar en tblEmpleado vinculado al usuario
                            int? sucursalSeleccionada = (int)cmbSucursal.SelectedValue;
                            if (sucursalSeleccionada == 0) sucursalSeleccionada = null;

                            var nuevoEmpleado = new Empleado
                            {
                                IdUsuario = nuevoUsuario.IdUsuario,
                                Sueldo = nudSueldo.Value,
                                FechaIngreso = dtpFechaIngreso.Value,
                                Estado = true,
                                IdSucursal = sucursalSeleccionada
                            };

                            db.Empleados.Add(nuevoEmpleado);
                            db.SaveChanges();

                            // 3. Confirmar todo
                            transaccion.Commit();
                            MessageBox.Show("Empleado registrado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LimpiarCampos();
                            CargarEmpleados();
                        }
                        catch (Exception)
                        {
                            transaccion.Rollback();
                            throw; // Lanza el error al catch principal
                        }
                    }
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
            if (idUsuarioSeleccionado == 0)
            {
                MessageBox.Show("Por favor, seleccione un empleado de la tabla para editarlo.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var db = new GomasContext())
                {
                    var usuarioObj = db.Usuarios.Find(idUsuarioSeleccionado);
                    var empleadoObj = db.Empleados.FirstOrDefault(emp => emp.IdUsuario == idUsuarioSeleccionado);

                    if (usuarioObj != null && empleadoObj != null)
                    {
                        // Actualizar Datos de Usuario
                        usuarioObj.TipoDocumento = string.IsNullOrWhiteSpace(txtTipoDocumento.Text) ? 0 : Convert.ToInt32(txtTipoDocumento.Text);
                        usuarioObj.Documento = txtDocumento.Text;
                        usuarioObj.Nombres = txtNombres.Text;
                        usuarioObj.Apellidos = txtApellidos.Text;
                        usuarioObj.Telefono = txtTelefono.Text;
                        usuarioObj.Correo = txtCorreo.Text;
                        usuarioObj.Rol = cmbRol.SelectedItem.ToString();

                        // Si ingresó una nueva contraseña, la hasheamos y actualizamos
                        if (!string.IsNullOrWhiteSpace(txtContraseña.Text))
                        {
                            usuarioObj.ClaveHash = SeguridadHelper.CalcularHash(txtContraseña.Text);
                        }

                        // Actualizar Datos de Empleado
                        int? sucursalSeleccionada = (int)cmbSucursal.SelectedValue;
                        if (sucursalSeleccionada == 0) sucursalSeleccionada = null;

                        empleadoObj.Sueldo = nudSueldo.Value;
                        empleadoObj.FechaIngreso = dtpFechaIngreso.Value;
                        empleadoObj.IdSucursal = sucursalSeleccionada;

                        db.SaveChanges();
                        MessageBox.Show("Empleado actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LimpiarCampos();
                        CargarEmpleados();
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
                MessageBox.Show("Por favor, seleccione un empleado de la tabla para eliminarlo.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult dialogo = MessageBox.Show("¿Está seguro que desea dar de baja a este empleado?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dialogo == DialogResult.Yes)
            {
                try
                {
                    using (var db = new GomasContext())
                    {
                        var usuarioObj = db.Usuarios.Find(idUsuarioSeleccionado);
                        var empleadoObj = db.Empleados.FirstOrDefault(emp => emp.IdUsuario == idUsuarioSeleccionado);

                        if (usuarioObj != null && empleadoObj != null)
                        {
                            // Soft Delete: Solo los desactivamos
                            usuarioObj.Estado = false;
                            empleadoObj.Estado = false;

                            db.SaveChanges();
                            MessageBox.Show("Empleado dado de baja exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LimpiarCampos();
                            CargarEmpleados();
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
        private void dgvEmpleados_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dgvEmpleados.Rows[e.RowIndex];

                idUsuarioSeleccionado = Convert.ToInt32(fila.Cells["IdUsuario"].Value);

                txtTipoDocumento.Text = fila.Cells["TipoDocumento"].Value?.ToString();
                txtDocumento.Text = fila.Cells["Documento"].Value?.ToString();
                txtNombres.Text = fila.Cells["Nombres"].Value?.ToString();
                txtApellidos.Text = fila.Cells["Apellidos"].Value?.ToString();
                txtTelefono.Text = fila.Cells["Telefono"].Value?.ToString();
                txtCorreo.Text = fila.Cells["Correo"].Value?.ToString();

                // Cargar Rol en el Combo
                string rol = fila.Cells["Rol"].Value?.ToString();
                if (cmbRol.Items.Contains(rol)) cmbRol.SelectedItem = rol;

                // Cargar Sucursal en el Combo
                if (fila.Cells["IdSucursal"].Value != null)
                    cmbSucursal.SelectedValue = Convert.ToInt32(fila.Cells["IdSucursal"].Value);
                else
                    cmbSucursal.SelectedValue = 0; // Sin asignar

                // Cargar Fechas y Sueldos
                if (fila.Cells["Sueldo"].Value != null)
                    nudSueldo.Value = Convert.ToDecimal(fila.Cells["Sueldo"].Value);

                if (fila.Cells["FechaIngreso"].Value != null)
                    dtpFechaIngreso.Value = Convert.ToDateTime(fila.Cells["FechaIngreso"].Value);

                txtContraseña.Clear(); // Nunca cargar la contraseña
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void LimpiarCampos()
        {
            idUsuarioSeleccionado = 0;
            txtTipoDocumento.Clear();
            txtDocumento.Clear();
            txtContraseña.Clear();
            txtNombres.Clear();
            txtApellidos.Clear();
            txtTelefono.Clear();
            txtCorreo.Clear();

            cmbRol.SelectedIndex = 0;
            if (cmbSucursal.Items.Count > 0) cmbSucursal.SelectedIndex = 0;

            nudSueldo.Value = 0;
            dtpFechaIngreso.Value = DateTime.Now;

            dgvEmpleados.ClearSelection();
            txtTipoDocumento.Focus();
        }
    }
}