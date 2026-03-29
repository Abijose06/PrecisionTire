using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebGomas.Models;

namespace WebGomas
{
    public partial class DetallePedido : System.Web.UI.Page
    {
        private List<Pedido> ObtenerPedidos()
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarDetalle();
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

        private void CargarDetalle()
        {
            // Leer y validar el id del QueryString
            string parametro = Request.QueryString["id"];
            int id;

            if (!int.TryParse(parametro, out id))
            {
                MostrarError();
                return;
            }

            // Buscar el pedido
            Pedido pedido = ObtenerPedidos().FirstOrDefault(p => p.Id == id);

            if (pedido == null)
            {
                MostrarError();
                return;
            }

            // Llenar los datos
            lblId.Text = "#" + pedido.Id;
            lblIdDetalle.Text = "#" + pedido.Id;
            lblFecha.Text = pedido.Fecha;
            lblTotal.Text = pedido.Total.ToString("C2");

            // Badge de estado con color
            lblEstado.Text = pedido.Estado;
            switch (pedido.Estado)
            {
                case "Completado": lblEstado.CssClass = "badge-estado estado-completado"; break;
                case "Procesando": lblEstado.CssClass = "badge-estado estado-procesando"; break;
                case "Pendiente": lblEstado.CssClass = "badge-estado estado-pendiente"; break;
            }

            pnlDetalle.Visible = true;
        }

        private void MostrarError()
        {
            pnlDetalle.Visible = false;
            pnlError.Visible = true;
        }
    }
}