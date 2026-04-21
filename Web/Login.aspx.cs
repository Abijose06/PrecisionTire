using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.Net;
using System.Net.Http;          
using System.Text;
using System.Web;
using System.Web.UI;

namespace WebGomas
{
    public partial class Login : System.Web.UI.Page
    {
        // -------------------------------------------------------
        // Carga inicial — si ya hay sesión activa, redirigir
        // -------------------------------------------------------
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Agregamos HttpContext.Current para evitar el error CS0103
                if (HttpContext.Current.Session["usuario"] != null)
                {
                    HttpContext.Current.Response.Redirect("Productos.aspx");
                }
            }
        }

        // -------------------------------------------------------
        // Clic en "Iniciar sesión"
        // -------------------------------------------------------
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string email = txtEmail.Text.Trim().ToLower();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MostrarError("Por favor completa todos los campos.");
                return;
            }

            if (!email.Contains("@"))
            {
                MostrarError("Ingresa un correo electrónico válido.");
                return;
            }

            try
            {

                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                using (HttpClient client = new HttpClient())
                {
                    string UrlIntegracion = ConfigurationManager.AppSettings["UrlIntegracion"];

                    var requestData = new
                    {
                        Correo = email,
                        Password = password
                    };

                    string json = JsonConvert.SerializeObject(requestData);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    // Ahora PostAsync funcionará porque agregamos el using System.Net.Http arriba
                    var response = client.PostAsync(UrlIntegracion, content).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var resultString = response.Content.ReadAsStringAsync().Result;

                        // Usamos JObject en vez de dynamic para evitar el error CS0246
                        JObject resultData = JObject.Parse(resultString);

                        // Guardamos los datos de sesión devueltos por la base de datos
                        HttpContext.Current.Session["usuario"] = email;

                        if (resultData["NombreCompleto"] != null)
                        {
                            HttpContext.Current.Session["nombre"] = resultData["NombreCompleto"].ToString();
                        }

                        if (resultData["IdCliente"] != null)
                            HttpContext.Current.Session["idCliente"] = resultData["IdCliente"].ToString();

                        HttpContext.Current.Response.Redirect("Productos.aspx", false);
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        MostrarError("El correo o la contraseña son incorrectos.");
                    }
                    else
                    {
                        MostrarError("Error al intentar iniciar sesión. Intenta más tarde.");
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarError("No se pudo conectar con el servidor: " + ex.Message);
            }
        }

        // -------------------------------------------------------
        // Muestra el panel de error con el mensaje indicado
        // -------------------------------------------------------
        private void MostrarError(string mensaje)
        {
            lblError.Text = mensaje;
            pnlError.Visible = true;
        }
    }
}