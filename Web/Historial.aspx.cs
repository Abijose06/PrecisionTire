using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebGomas.Models;

namespace WebGomas
{
    public partial class Historial : System.Web.UI.Page
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
            {
                ActualizarHeader();
                CargarHistorial();
            }
        }

        private void CargarHistorial()
        {
            List<Pedido> pedidos = new List<Pedido>(); // lista vacía por defecto

            try
            {
                int idCliente = 0;

                if (Session["idCliente"] != null)
                    int.TryParse(Session["idCliente"].ToString(), out idCliente);

                if (idCliente > 0)
                    pedidos = ObtenerHistorialDesdeAPI(idCliente);
            }
            catch { }

            gvHistorial.DataSource = pedidos;
            gvHistorial.DataBind();

            decimal gastoTotal = pedidos.Sum(p => p.Total);
            int completados = pedidos.Count(p => p.Estado == "Completado" || p.Estado == "Pagada");
            int proximoServ = completados > 0 ? (completados / 3 + 1) * 3 : 3;

            lblGastoTotal.Text = gastoTotal.ToString("C2");
            lblPedidosCompletados.Text = completados + " pedidos";
            lblProximoServicio.Text = "Al pedido #" + proximoServ;
            lblTotalPedidos.Text = pedidos.Count + " pedidos";
        }

        private List<Pedido> ObtenerHistorialDesdeAPI(int idCliente)
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                using (var client = new HttpClient())
                {
                    var response = client.GetAsync(UrlIntegracion + "facturacion/historial/" + idCliente).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var json = response.Content.ReadAsStringAsync().Result;
                        var dtos = JsonConvert.DeserializeObject<List<PedidoDTO>>(json);

                        var lista = new List<Pedido>();
                        foreach (var dto in dtos)
                        {
                            lista.Add(new Pedido
                            {
                                Id = dto.IdFactura,
                                Fecha = dto.Fecha.ToString("dd/MM/yyyy"),
                                Total = dto.TotalGeneral,
                                Estado = dto.EstadoFactura
                            });
                        }
                        return lista;
                    }
                }
            }
            catch { }

            return new List<Pedido>(); // ← lista vacía en vez de simulados
        }

        private List<Pedido> ObtenerDatosSimulados()
        {
            return new List<Pedido>
            {
                new Pedido { Id = 1001, Fecha = "01/03/2026", Total = 185.00m, Estado = "Completado" },
                new Pedido { Id = 1002, Fecha = "05/03/2026", Total = 340.00m, Estado = "Completado" },
                new Pedido { Id = 1003, Fecha = "10/03/2026", Total = 155.00m, Estado = "Pendiente"  },
                new Pedido { Id = 1004, Fecha = "15/03/2026", Total = 290.00m, Estado = "Procesando" },
                new Pedido { Id = 1005, Fecha = "20/03/2026", Total = 170.00m, Estado = "Completado" }
            };
        }

        protected void gvHistorial_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;

            Label lblEstado = e.Row.FindControl("lblEstado") as Label;
            if (lblEstado == null) return;

            switch (lblEstado.Text)
            {
                case "Completado":
                case "Pagada":
                    lblEstado.CssClass = "badge-estado estado-completado"; break;
                case "Procesando":
                    lblEstado.CssClass = "badge-estado estado-procesando"; break;
                case "Pendiente":
                    lblEstado.CssClass = "badge-estado estado-pendiente"; break;
            }
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

        private class PedidoDTO
        {
            public int IdFactura { get; set; }
            public DateTime Fecha { get; set; }
            public decimal TotalGeneral { get; set; }
            public string EstadoFactura { get; set; }
        }
    }
}