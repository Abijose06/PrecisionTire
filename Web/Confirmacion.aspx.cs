using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.UI;
using Newtonsoft.Json;
using WebGomas.Models;

namespace WebGomas
{
    public partial class Confirmacion : System.Web.UI.Page
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
                MostrarResumen();
            }
        }

        private void MostrarResumen()
        {
            List<CarritoItem> carrito = Session["carrito"] as List<CarritoItem>;

            if (carrito == null || carrito.Count == 0)
            {
                pnlVacio.Visible = true;
                pnlConfirmacion.Visible = false;
                return;
            }

            gvConfirmacion.DataSource = carrito;
            gvConfirmacion.DataBind();

            decimal total = carrito.Sum(item => item.Subtotal);
            lblTotal.Text = total.ToString("C2");
            lblMensaje.Text = string.Empty;

            pnlMensajeExito.Visible = false;
            pnlConfirmacion.Visible = true;
        }

        protected void btnConfirmar_Click(object sender, EventArgs e)
        {
            List<CarritoItem> carrito = Session["carrito"] as List<CarritoItem>;

            if (carrito == null || carrito.Count == 0)
            {
                Response.Redirect("Carrito.aspx");
                return;
            }

            decimal total = carrito.Sum(item => item.Subtotal);
            bool guardado = EnviarPedidoACore(carrito);

            Session["carrito"] = null;

            lblTotal.Text = total.ToString("C2");
            lblMensaje.Text = guardado
                ? "¡Compra realizada con éxito!"
                : "¡Compra registrada! (modo offline)";

            pnlMensajeExito.Visible = true;
            btnConfirmar.Visible = false;
            btnVolverCarrito.Visible = false;
            pnlAccionesFinales.Visible = true;
        }

        private bool EnviarPedidoACore(List<CarritoItem> carrito)
        {
            try
            {
                int idCliente = 0;
                int idSucursal = 1;

                if (Session["idCliente"] != null)
                    int.TryParse(Session["idCliente"].ToString(), out idCliente);

                if (idCliente <= 0)
                    return false;

                var request = new
                {
                    IdCliente = idCliente,
                    IdSucursal = idSucursal,
                    MetodoPago = "Efectivo",
                    IdVehiculo = 1,
                    Detalles = carrito.Select(item => new
                    {
                        TipoItem = "P",
                        IdProducto = item.ProductoId,
                        Cantidad = item.Cantidad,
                        PrecioUnitario = item.Precio
                    }).ToList()
                };

                string json = JsonConvert.SerializeObject(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                using (var client = new HttpClient())
                {
                    var response = client.PostAsync(URL_CORE + "facturacion/procesar", content).Result;
                    return response.IsSuccessStatusCode;
                }
            }
            catch
            {
                return false;
            }
        }

        protected void btnVolverCarrito_Click(object sender, EventArgs e)
        {
            Response.Redirect("Carrito.aspx");
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
    }
}