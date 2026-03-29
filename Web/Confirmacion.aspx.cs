using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebGomas.Models;

namespace WebGomas
{
    public partial class Confirmacion : System.Web.UI.Page
    {

       

        // -------------------------------------------------------
        // Carga inicial 
        // -------------------------------------------------------
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                MostrarResumen();
            }

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
        // Lee el carrito y llena la vista.
        // No limpia la sesión — el usuario aún no confirmó.
        // -------------------------------------------------------
        private void MostrarResumen()
        {
            List<CarritoItem> carrito = Session["carrito"] as List<CarritoItem>;

            // Si el carrito está vacío, mostrar panel de aviso
            if (carrito == null || carrito.Count == 0)
            {
                pnlVacio.Visible = true;
                pnlConfirmacion.Visible = false;
                return;
            }

            // Llenar el GridView con los productos del carrito
            gvConfirmacion.DataSource = carrito;
            gvConfirmacion.DataBind();

            // Calcular y mostrar el total
            decimal total = carrito.Sum(item => item.Subtotal);
            lblTotal.Text = total.ToString("C2");

            // El mensaje de éxito permanece oculto hasta que el usuario confirme
            lblMensaje.Text = string.Empty;
            pnlMensajeExito.Visible = false;

            // Mostrar panel principal
            pnlConfirmacion.Visible = true;
        }

        // -------------------------------------------------------
        // Clic en "Confirmar compra":
        // -------------------------------------------------------
        protected void btnConfirmar_Click(object sender, EventArgs e)
        {
            List<CarritoItem> carrito = Session["carrito"] as List<CarritoItem>;

            if (carrito == null || carrito.Count == 0)
            {
                Response.Redirect("Carrito.aspx");
                return;
            }

            // Guardar el total ANTES de limpiar la sesión
            decimal total = carrito.Sum(item => item.Subtotal);

            // Limpiar el carrito
            Session["carrito"] = null;

            // Mostrar total calculado
            lblTotal.Text = total.ToString("C2");

            // Mostrar mensaje de éxito
            lblMensaje.Text = "¡Compra realizada con éxito!";
            pnlMensajeExito.Visible = true;

            // Ocultar botones principales
            btnConfirmar.Visible = false;
            btnVolverCarrito.Visible = false;

            // ── NUEVO: mostrar acciones post-confirmación ──
            pnlAccionesFinales.Visible = true;
        }

        // -------------------------------------------------------
        // Clic en "Volver al carrito":
        // Regresa sin tocar la sesión — el carrito se conserva.
        // -------------------------------------------------------
        protected void btnVolverCarrito_Click(object sender, EventArgs e)
        {
            Response.Redirect("Carrito.aspx");
        }
    }
}

