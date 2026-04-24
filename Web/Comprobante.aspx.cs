using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Web.UI;

namespace WebGomas
{
    public partial class Comprobante : System.Web.UI.Page
    {
        string UrlIntegracion = ConfigurationManager.AppSettings["UrlIntegracion"];

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuario"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
                CargarComprobante();
        }

        private void CargarComprobante()
        {
            string parametro = Request.QueryString["id"];
            int id;

            if (!int.TryParse(parametro, out id))
                return;

            lblIdFactura.Text = "#" + id;
            lblFecha.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            lblCliente.Text = Session["nombre"] != null ? Session["nombre"].ToString() : "";

            try
            {
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                using (var client = new HttpClient())
                {
                    var response = client.GetAsync(UrlIntegracion + "Facturacion/Detalle/" + id).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var json = response.Content.ReadAsStringAsync().Result;
                        var detalles = JsonConvert.DeserializeObject<List<DetalleItemDTO>>(json);

                        rptItems.DataSource = detalles;
                        rptItems.DataBind();

                        decimal subtotal = 0;
                        foreach (var item in detalles)
                            subtotal += item.SubTotal;

                        decimal itbis = Math.Round(subtotal * 0.18m, 2);
                        decimal totalConItbis = Math.Round(subtotal + itbis, 2);

                        lblSubtotal.Text = subtotal.ToString("C2");
                        lblItbis.Text = itbis.ToString("C2");
                        lblTotal.Text = totalConItbis.ToString("C2");
                    }
                }
            }
            catch { }
        }

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