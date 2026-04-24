using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Net.Http;
using System.Windows.Forms;

namespace Core.DataEntry
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        public string RolUsuario = "";
        private string UrlIntegracion = ConfigurationManager.AppSettings["UrlIntegracion"];

        private async void btnIngresar_Click(object sender, EventArgs e)
        {
            btnIngresar.Enabled = false;
            btnIngresar.Text = "Conectando...";

            var peticionLogin = new
            {
                Correo = txtUsuario.Text,
                Password = txtPassword.Text
            };

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(UrlIntegracion);
                    client.Timeout = TimeSpan.FromSeconds(4);

                    var content = new StringContent(JsonConvert.SerializeObject(peticionLogin), System.Text.Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("api/usuarios/login", content);

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();
                        dynamic usuario = JsonConvert.DeserializeObject(jsonString);

                        this.RolUsuario = usuario.Rol;
                        MessageBox.Show("Bienvenido, " + usuario.NombreCompleto, "Acceso Correcto", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        MessageBox.Show("Correo o contraseña incorrectos.", "Acceso Denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        MessageBox.Show("Error al conectar con el servidor.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("No se pudo conectar al servidor. Verifique que el sistema esté corriendo.", "Sin Conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            btnIngresar.Enabled = true;
            btnIngresar.Text = "Iniciar Sesión";
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
        }
    }
}