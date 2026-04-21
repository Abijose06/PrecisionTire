using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.UI;
using Newtonsoft.Json;
using WebGomas.Models;

namespace WebGomas
{
    public partial class DetallePedido : System.Web.UI.Page
    {
        private const string URL_CORE = "https://localhost:44376/api/";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuario"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                ActualizarHeader();
                CargarDetalle();
            }
        }

        private void CargarDetalle()
        {
            string parametro = Request.QueryString["id"];
            int id;

            if (!int.TryParse(parametro, out id))
            {
                MostrarError();
                return;
            }

            // Obtener detalle de la factura desde Core
            List<DetalleItemDTO> detalles = ObtenerDetalleDesdeAPI(id);

            if (detalles == null || detalles.Count == 0)
            {
                MostrarError();
                return;
            }

            // Llenar cabecera
            lblId.Text = "#" + id;
            lblIdDetalle.Text = "#" + id;

            // Calcular total
            decimal total = 0;
            foreach (var item in detalles)
                total += item.SubTotal;

            lblTotal.Text = total.ToString("C2");
            lblTotalGeneral.Text = total.ToString("C2");

            // Estado fijo — viene de la factura
            lblEstado.Text = "Pagada";
            lblEstado.CssClass = "badge-estado estado-pagada";

            // Llenar GridView con los productos
            gvProductos.DataSource = detalles;
            gvProductos.DataBind();

            pnlDetalle.Visible = true;
        }

        private List<DetalleItemDTO> ObtenerDetalleDesdeAPI(int idFactura)
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                using (var client = new HttpClient())
                {
                    var response = client.GetAsync(URL_CORE + "facturacion/detalle/" + idFactura).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var json = response.Content.ReadAsStringAsync().Result;
                        return JsonConvert.DeserializeObject<List<DetalleItemDTO>>(json);
                    }
                }
            }
            catch { }

            return null;
        }

        private void MostrarError()
        {
            pnlDetalle.Visible = false;
            pnlError.Visible = true;
        }

        private void ActualizarHeader()
        {
            if (Session["usuario"] != null)
            {
                phLogueado.Visible = true;
                phNoLogueado.Visible = false;
                string nombre = Session["nombre"].ToString();
                lblUsuario.Text = nombre;
                lblAvatar.Text = nombre.Substring(0, 1).ToUpper();
            }
            else
            {
                phLogueado.Visible = false;
                phNoLogueado.Visible = true;
            }
        }

        // DTO que coincide con lo que devuelve Core
        private class DetalleItemDTO
        {
            public string Marca { get; set; }
            public string Modelo { get; set; }
            public string Medida { get; set; }
            public int Cantidad { get; set; }
            public decimal PrecioUnitario { get; set; }
            public decimal SubTotal { get; set; }
        }
    }
}