using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.UI;

namespace WebGomas
{
    public partial class Registro : System.Web.UI.Page
    {
        string UrlIntegracion = ConfigurationManager.AppSettings["UrlIntegracion"];

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Si ya está logueado, redirigir al catálogo
                if (HttpContext.Current.Session["usuario"] != null)
                    HttpContext.Current.Response.Redirect("Productos.aspx");
            }
        }

        protected void btnRegistrar_Click(object sender, EventArgs e)
        {
            // Validar campos vacíos
            if (string.IsNullOrEmpty(txtNombres.Text.Trim()) ||
                string.IsNullOrEmpty(txtApellidos.Text.Trim()) ||
                string.IsNullOrEmpty(txtCorreo.Text.Trim()) ||
                string.IsNullOrEmpty(txtTelefono.Text.Trim()) ||
                string.IsNullOrEmpty(txtDocumento.Text.Trim()) ||
                string.IsNullOrEmpty(txtPassword.Text.Trim()))
            {
                MostrarError("Por favor completa todos los campos.");
                return;
            }

            if (!txtCorreo.Text.Contains("@"))
            {
                MostrarError("Ingresa un correo electrónico válido.");
                return;
            }

            if (txtPassword.Text.Trim().Length < 6)
            {
                MostrarError("La contraseña debe tener al menos 6 caracteres.");
                return;
            }

            try
            {
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                using (var client = new HttpClient())
                {
                    // Armar el objeto que espera Core
                    var requestData = new
                    {
                        TipoDocumento = 1,
                        Documento = txtDocumento.Text.Trim(),
                        Nombres = txtNombres.Text.Trim(),
                        Apellidos = txtApellidos.Text.Trim(),
                        Telefono = txtTelefono.Text.Trim(),
                        Correo = txtCorreo.Text.Trim().ToLower(),
                        Password = txtPassword.Text.Trim(),
                        Rol = "ClienteWeb",
                        Direccion = txtDireccion.Text.Trim()
                    };

                    string json = JsonConvert.SerializeObject(requestData);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = client.PostAsync(UrlIntegracion + "usuarios/registro", content).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        // Registro exitoso — mostrar mensaje y redirigir al login
                        pnlError.Visible = false;
                        MostrarExito("¡Cuenta creada con éxito! Redirigiendo al login...");

                        // Redirigir al login después de 2 segundos
                        Response.AddHeader("Refresh", "2;url=Login.aspx");
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        MostrarError("Datos incorrectos. Verifica la información ingresada.");
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                    {
                        MostrarError("Este correo o documento ya está registrado.");
                    }
                    else
                    {
                        MostrarError("Error al crear la cuenta. Intenta más tarde.");
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarError("No se pudo conectar con el servidor: " + ex.Message);
            }
        }

        private void MostrarError(string mensaje)
        {
            lblError.Text = mensaje;
            pnlError.Visible = true;
            pnlExito.Visible = false;
        }

        private void MostrarExito(string mensaje)
        {
            lblExito.Text = mensaje;
            pnlExito.Visible = true;
            pnlError.Visible = false;
        }
    }
}