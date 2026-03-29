using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebGomas.Models;

namespace WebGomas
{

    public partial class Productos : System.Web.UI.Page
    {
        // -------------------------------------------------------
        // Datos simulados (hardcoded) — sin base de datos
        // -------------------------------------------------------
        private List<Producto> ObtenerProductos()
        {
            return new List<Producto>
            {
                new Producto { Id = 1, Nombre = "Pilot Sport 4",     Marca = "Michelin",    Precio = 185.00m,   ImagenUrl = "images/GomaPilotSport4.png"},
                new Producto { Id = 2, Nombre = "Eagle F1 Asymmetric", Marca = "Goodyear",  Precio = 160.00m,   ImagenUrl = "images/GomaEagle F1 Asymmetric.png"},
                new Producto { Id = 3, Nombre = "Cinturato P7",      Marca = "Pirelli",     Precio = 145.00m,   ImagenUrl = "images/GomaCinturatoP7.png"},
                new Producto { Id = 4, Nombre = "ContiSportContact", Marca = "Continental", Precio = 170.00m,   ImagenUrl = "images/GomaContiSportContact.png"},
                new Producto { Id = 5, Nombre = "Potenza S007",      Marca = "Bridgestone", Precio = 155.00m,   ImagenUrl = "images/GomaPotenza S007.png"},
                new Producto { Id = 6, Nombre = "Ventus S1 Evo3",    Marca = "Hankook",     Precio = 138.00m,   ImagenUrl = "images/GomaVentusS1Evo3.png"},
                new Producto { Id = 7, Nombre = "Proxes Sport",      Marca = "Toyo",        Precio = 142.00m,   ImagenUrl = "images/GomaProxes Sport.png"},
                new Producto { Id = 8, Nombre = "Primacy 4+",        Marca = "Michelin",    Precio = 167.00m,   ImagenUrl = "images/GomaPrimacy4.png"}
            };
        }

        // -------------------------------------------------------
        // Carga inicial de la página
        // -------------------------------------------------------
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarProductos();
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
        // Vincula la lista al Repeater y actualiza el contador
        // -------------------------------------------------------
        private void CargarProductos()
        {
            List<Producto> productos = ObtenerProductos();

            // Mostrar cantidad de productos en el badge del header
            lblCantidad.Text = productos.Count + " productos";

            // Binding al Repeater
            rptProductos.DataSource = productos;
            rptProductos.DataBind();
        }

    }
}
