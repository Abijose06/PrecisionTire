using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebGomas.Models;

namespace WebGomas
{
    public partial class Historial : System.Web.UI.Page
    {
        // -------------------------------------------------------
        // Datos simulados
        // -------------------------------------------------------
        private List<Pedido> ObtenerPedidos()
        {
            return new List<Pedido>
            {
                new Pedido { Id = 1001, Fecha = "01/03/2026", Total = 185.00m, Estado = "Completado"  },
                new Pedido { Id = 1002, Fecha = "05/03/2026", Total = 340.00m, Estado = "Completado"  },
                new Pedido { Id = 1003, Fecha = "10/03/2026", Total = 155.00m, Estado = "Pendiente"   },
                new Pedido { Id = 1004, Fecha = "15/03/2026", Total = 290.00m, Estado = "Procesando"  },
                new Pedido { Id = 1005, Fecha = "20/03/2026", Total = 170.00m, Estado = "Completado"  }
            };
        }

        // -------------------------------------------------------
        // Carga inicial
        // -------------------------------------------------------
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarHistorial();
            }

            // Proteger la página
            if (Session["usuario"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                ActualizarHeader();    // ← agregar esta línea
                                       // ... tu código existente que ya tenías
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

        // -------------------------------------------------------
        // Llena la tabla y calcula las estadísticas
        // -------------------------------------------------------
        private void CargarHistorial()
        {
            List<Pedido> pedidos = ObtenerPedidos();

            // Binding al GridView
            gvHistorial.DataSource = pedidos;
            gvHistorial.DataBind();

            // ── Estadísticas ──

            // Gasto total
            decimal gastoTotal = pedidos.Sum(p => p.Total);
            lblGastoTotal.Text = gastoTotal.ToString("C2");

            // Pedidos completados
            int completados = pedidos.Count(p => p.Estado == "Completado");
            lblPedidosCompletados.Text = completados + " pedidos";

            // Próximo servicio (cada 3 pedidos completados)
            int proximoServicio = (completados / 3 + 1) * 3;
            lblProximoServicio.Text = "Al pedido #" + proximoServicio;

            // Badge con total de pedidos
            lblTotalPedidos.Text = pedidos.Count + " pedidos";
        }

        // -------------------------------------------------------
        // Colorea el badge de estado fila por fila
        // -------------------------------------------------------
        protected void gvHistorial_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
                return;

            Label lblEstado = e.Row.FindControl("lblEstado") as Label;

            if (lblEstado == null)
                return;

            switch (lblEstado.Text)
            {
                case "Completado":
                    lblEstado.CssClass = "badge-estado estado-completado";
                    break;
                case "Procesando":
                    lblEstado.CssClass = "badge-estado estado-procesando";
                    break;
                case "Pendiente":
                    lblEstado.CssClass = "badge-estado estado-pendiente";
                    break;
            }
        }
    }
}