using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebGomas
{
    public partial class Login : System.Web.UI.Page
    {
        // -------------------------------------------------------
        // Usuarios simulados — sin base de datos
        // Cada entrada es: email → contraseña
        // -------------------------------------------------------
        private Dictionary<string, string> ObtenerUsuarios()
        {
            return new Dictionary<string, string>
            {
                { "admin@precisiontire.com", "admin123" },
                { "jorge@precisiontire.com", "jorge123" },
                { "test@test.com",           "test123"  }
            };
        }

        // -------------------------------------------------------
        // Carga inicial — si ya hay sesión activa, redirigir
        // -------------------------------------------------------
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Si el usuario ya está logueado, ir directo al catálogo
                if (Session["usuario"] != null)
                {
                    Response.Redirect("Productos.aspx");
                }
            }
        }

        // -------------------------------------------------------
        // Clic en "Iniciar sesión"
        // -------------------------------------------------------
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim().ToLower();
            string password = txtPassword.Text.Trim();

            // Validar que los campos no estén vacíos
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MostrarError("Por favor completa todos los campos.");
                return;
            }

            // Validar formato de email básico
            if (!email.Contains("@"))
            {
                MostrarError("Ingresa un correo electrónico válido.");
                return;
            }

            // Buscar el usuario en la lista simulada
            Dictionary<string, string> usuarios = ObtenerUsuarios();

            if (!usuarios.ContainsKey(email))
            {
                MostrarError("El correo electrónico no está registrado.");
                return;
            }

            if (usuarios[email] != password)
            {
                MostrarError("La contraseña es incorrecta.");
                return;
            }

            // Login exitoso — guardar en Session y redirigir
            Session["usuario"] = email;
            Session["nombre"] = ObtenerNombre(email);

            Response.Redirect("Productos.aspx");
        }

        // -------------------------------------------------------
        // Muestra el panel de error con el mensaje indicado
        // -------------------------------------------------------
        private void MostrarError(string mensaje)
        {
            lblError.Text = mensaje;
            pnlError.Visible = true;
        }

        // -------------------------------------------------------
        // Devuelve el nombre amigable según el email
        // -------------------------------------------------------
        private string ObtenerNombre(string email)
        {
            Dictionary<string, string> nombres = new Dictionary<string, string>
            {
                { "admin@precisiontire.com", "Administrador" },
                { "jorge@precisiontire.com", "Jorge"         },
                { "test@test.com",           "Usuario Test"  }
            };

            return nombres.ContainsKey(email) ? nombres[email] : email;
        }
    }
}