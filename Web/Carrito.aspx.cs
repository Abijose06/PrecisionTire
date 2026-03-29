using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebGomas.Models
{
    public partial class Carrito : System.Web.UI.Page
    {
        // -------------------------------------------------------
        // Carga inicial
        // -------------------------------------------------------
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                CargarCarrito();
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
        // Lee el carrito desde Session y llena la vista
        // -------------------------------------------------------
        private void CargarCarrito()
        {
            List<CarritoItem> carrito = Session["carrito"] as List<CarritoItem>;

            if (carrito == null || carrito.Count == 0)
            {
                pnlVacio.Visible = true;
                pnlCarrito.Visible = false;
                return;
            }

            // Binding al Repeater
            rptCarrito.DataSource = carrito;
            rptCarrito.DataBind();

            // Calcular totales
            decimal total = carrito.Sum(item => item.Subtotal);

            lblTotal.Text = total.ToString("C2");
            lblSubtotal.Text = total.ToString("C2");
            lblCantidadItems.Text = carrito.Count + (carrito.Count == 1 ? " producto" : " productos");

            pnlCarrito.Visible = true;
            pnlVacio.Visible = false;
        }

        // -------------------------------------------------------
        // Maneja los botones +, - y ✕ del Repeater
        // -------------------------------------------------------
        protected void rptCarrito_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            // Obtener el carrito actual
            List<CarritoItem> carrito = Session["carrito"] as List<CarritoItem>;

            if (carrito == null)
                return;

            // Leer el ProductoId enviado por CommandArgument
            int productoId = Convert.ToInt32(e.CommandArgument);

            // Buscar el item en el carrito
            CarritoItem item = carrito.FirstOrDefault(c => c.ProductoId == productoId);

            if (item == null)
                return;

            switch (e.CommandName)
            {
                case "Sumar":
                    // Aumentar cantidad en 1
                    item.Cantidad += 1;
                    break;

                case "Restar":
                    // Bajar cantidad en 1 — si llega a 0, eliminar el item
                    item.Cantidad -= 1;
                    if (item.Cantidad <= 0)
                        carrito.Remove(item);
                    break;

                case "Eliminar":
                    // Eliminar el item directamente
                    carrito.Remove(item);
                    break;
            }

            // Guardar el carrito actualizado en Session
            Session["carrito"] = carrito;

            // Recargar la vista con los datos nuevos
            CargarCarrito();
        }

        // -------------------------------------------------------
        // Clic en "Confirmar compra"
        // -------------------------------------------------------
        protected void btnConfirmar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Confirmacion.aspx");
        }

        // -------------------------------------------------------
        // Clic en "Eliminar todo"
        // -------------------------------------------------------
        protected void btnEliminarTodo_Click(object sender, EventArgs e)
        {
            Session["carrito"] = null;
            CargarCarrito();
        }

       
    }
}
