using System;
using System.Windows.Forms;

namespace CajaGomasPOS
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            // 👉 PARA EL EQUIPO DE INTEGRACIÓN:
            // Esta validación está quemada para la maqueta. 
            // Aquí deben consumir el Servicio Web del CORE que valida 
            // los usuarios y contraseñas reales de la base de datos.
            // Ejemplo: if (API.LoginUsuario(txtUsuario.Text, txtPassword.Text))

            if (txtUsuario.Text == "admin" && txtPassword.Text == "1234") // <--- Reemplazar esto
            {
                Form1 pantallaCaja = new Form1();

                // Le pasamos el fondo de caja que escribió el cajero a la pantalla principal
                try { pantallaCaja.FondoCaja = Convert.ToDecimal(txtFondo.Text); }
                catch { pantallaCaja.FondoCaja = 0; }

                pantallaCaja.Show();
                this.Hide(); // Ocultamos el login
            }
            else
            {
                // Si el Servicio Web responde que la clave es incorrecta:
                MessageBox.Show("Usuario o contraseña incorrectos. Intente de nuevo.", "Error de Acceso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Clear();
                txtUsuario.Focus();
            }
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            // Puedes dejar esto vacío si no necesitas cargar nada al inicio
        }
    }
}