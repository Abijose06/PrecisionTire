using Core.Helpers;
using Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Core.DataEntry
{
    public partial class frmCliente : Form
    {
        public frmCliente()
        {
            InitializeComponent();
        }

        private void btnInsertarCliente_Click(object sender, EventArgs e)
        {
            // 1. Validación básica para evitar que se vaya en blanco
            if (string.IsNullOrWhiteSpace(txtDocumento.Text) ||
                string.IsNullOrWhiteSpace(txtContraseña.Text) ||
                string.IsNullOrWhiteSpace(txtNombres.Text))
            {
                MessageBox.Show("Por favor llene los campos obligatorios.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // 2. Instanciamos el contexto de Entity Framework
                using (var db = new GomasContext())
                {
                    // 3. Iniciamos la transacción (Todo o Nada)
                    using (var transaccion = db.Database.BeginTransaction())
                    {
                        try
                        {
                            // 4. Creamos el objeto Usuario primero
                            var nuevoUsuario = new Usuario
                            {
                                // Convertimos el texto a número (Ej. 1 para Cédula)
                                TipoDocumento = Convert.ToInt32(txtTipoDocumento.Text),
                                Documento = txtDocumento.Text,

                                // Usamos tu clase de seguridad para hashear la contraseña
                                ClaveHash = SeguridadHelper.CalcularHash(txtContraseña.Text),

                                Rol = "Cliente", // Fijo por defecto como pediste
                                Nombres = txtNombres.Text,
                                Apellidos = txtApellidos.Text,
                                Telefono = txtTelefono.Text,
                                Correo = txtCorreo.Text,
                                Estado = true // Lo creamos activo por defecto
                            };

                            // Lo agregamos a la BD y guardamos para que SQL nos genere el IdUsuario
                            db.Usuarios.Add(nuevoUsuario);
                            db.SaveChanges();

                            // 5. Ahora creamos el objeto Cliente
                            var nuevoCliente = new Cliente
                            {
                                // ¡AQUÍ ESTÁ LA MAGIA! Tomamos el ID que se acaba de generar
                                IdUsuario = nuevoUsuario.IdUsuario,
                                Direccion = txtDireccion.Text
                            };

                            db.Clientes.Add(nuevoCliente);
                            db.SaveChanges();

                            // 6. Si llegamos aquí sin errores, confirmamos el guardado de ambas tablas
                            transaccion.Commit();

                            MessageBox.Show("El cliente ha sido registrado exitosamente en el sistema.", "Operación Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Limpiamos los campos para el siguiente registro
                            LimpiarCampos();
                        }
                        catch (Exception ex)
                        {
                            // Si algo falló (ej. error de SQL), cancelamos todo
                            transaccion.Rollback();
                            MessageBox.Show("Error al guardar en la base de datos: " + ex.Message, "Error de BD", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Verifique que el Tipo de Documento sea un número válido. Error: " + ex.Message, "Error de Datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Método auxiliar para vaciar las cajas de texto rápidamente
        private void LimpiarCampos()
        {
            txtTipoDocumento.Clear();
            txtDocumento.Clear();
            txtContraseña.Clear();
            txtNombres.Clear();
            txtApellidos.Clear();
            txtTelefono.Clear();
            txtCorreo.Clear();
            txtDireccion.Clear();
        }
    }
}
