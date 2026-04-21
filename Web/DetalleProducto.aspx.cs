using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.UI;
using WebGomas.Models;

namespace WebGomas
{
    public partial class DetalleProducto : System.Web.UI.Page
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
                CargarDetalle();
            }
        }

        private void CargarDetalle()
        {
            string parametroId = Request.QueryString["id"];
            int id;

            if (!int.TryParse(parametroId, out id))
            {
                MostrarError();
                return;
            }

            Producto producto = ObtenerProductoPorIdDesdeAPI(id);

            if (producto == null)
            {
                MostrarError();
                return;
            }

            lblNombre.Text = producto.Nombre;
            lblMarca.Text = producto.Marca;
            lblPrecio.Text = producto.Precio.ToString("C2");
            imgProducto.ImageUrl = producto.ImagenUrl;

            pnlDetalle.Visible = true;
        }

        private Producto ObtenerProductoPorIdDesdeAPI(int id)
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                using (var client = new HttpClient())
                {
                    var response = client.GetAsync(UrlIntegracion + "productos/detalle/" + id).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var json = response.Content.ReadAsStringAsync().Result;
                        var dto = JsonConvert.DeserializeObject<ProductoDTO>(json);

                        return new Producto
                        {
                            Id = dto.IdProducto,
                            Nombre = dto.Modelo,
                            Marca = dto.Marca,
                            Precio = dto.PrecioVenta,
                            ImagenUrl = "~/images/GomaPilotSport4.png"
                        };
                    }
                }
            }
            catch { }

            return ObtenerDatosSimulados().FirstOrDefault(p => p.Id == id);
        }

        private void MostrarError()
        {
            pnlDetalle.Visible = false;
            pnlError.Visible = true;
        }

        protected void btnAgregarCarrito_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(Request.QueryString["id"]);

            Producto producto = ObtenerProductoPorIdDesdeAPI(id);

            if (producto == null)
                return;

            int cantidad;
            if (!int.TryParse(txtCantidad.Text, out cantidad) || cantidad < 1)
                cantidad = 1;

            List<CarritoItem> carrito = Session["carrito"] as List<CarritoItem>;
            if (carrito == null)
                carrito = new List<CarritoItem>();

            CarritoItem itemExistente = carrito.FirstOrDefault(c => c.ProductoId == producto.Id);

            if (itemExistente != null)
            {
                itemExistente.Cantidad += cantidad;
            }
            else
            {
                carrito.Add(new CarritoItem
                {
                    ProductoId = producto.Id,
                    Nombre = producto.Nombre,
                    Precio = producto.Precio,
                    Cantidad = cantidad,
                    ImagenUrl = producto.ImagenUrl
                });
            }

            Session["carrito"] = carrito;
            Response.Redirect("Carrito.aspx");
        }

        private List<Producto> ObtenerDatosSimulados()
        {
            return new List<Producto>
            {
                new Producto { Id = 1, Nombre = "Pilot Sport 4",       Marca = "Michelin",    Precio = 185.00m, ImagenUrl = "images/GomaPilotSport4.png"       },
                new Producto { Id = 2, Nombre = "Eagle F1 Asymmetric", Marca = "Goodyear",    Precio = 160.00m, ImagenUrl = "images/GomaEagleF1Asymmetric.png" },
                new Producto { Id = 3, Nombre = "Cinturato P7",        Marca = "Pirelli",     Precio = 145.00m, ImagenUrl = "images/GomaCinturatoP7.png"       },
                new Producto { Id = 4, Nombre = "ContiSportContact",   Marca = "Continental", Precio = 170.00m, ImagenUrl = "images/GomaContiSportContact.png" },
                new Producto { Id = 5, Nombre = "Potenza S007",        Marca = "Bridgestone", Precio = 155.00m, ImagenUrl = "images/GomaPotenzaS007.png"       },
                new Producto { Id = 6, Nombre = "Ventus S1 Evo3",      Marca = "Hankook",     Precio = 138.00m, ImagenUrl = "images/GomaVentusS1Evo3.png"      },
                new Producto { Id = 7, Nombre = "Proxes Sport",        Marca = "Toyo",        Precio = 142.00m, ImagenUrl = "images/GomaProxesSport.png"       },
                new Producto { Id = 8, Nombre = "Primacy 4+",          Marca = "Michelin",    Precio = 167.00m, ImagenUrl = "images/GomaPrimacy4.png"          }
            };
        }

        private class ProductoDTO
        {
            public int IdProducto { get; set; }
            public string Marca { get; set; }
            public string Modelo { get; set; }
            public string Medida { get; set; }
            public decimal PrecioVenta { get; set; }
            public int StockActual { get; set; }
        }
    }
}