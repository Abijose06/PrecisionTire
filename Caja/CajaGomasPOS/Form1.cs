using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CajaGomasPOS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string reciboMetodo = "";
        decimal reciboEfectivo = 0;
        decimal reciboDevuelta = 0;

        public decimal FondoCaja = 0;
        public decimal TotalEfectivoDelDia = 0;

        // 👉 PARA EL EQUIPO DE INTEGRACIÓN (1/3): 
        // Esta lista simula la tabla 'Producto'. Cuando conecten la Base de Datos, 
        // deben borrar esta lista y llenar el 'cmbBuscarItem' haciendo un SELECT 
        // a la BD concatenando Marca + Modelo + Medida.
        List<string> inventarioGomas = new List<string>
        {
            "Michelin Pilot Sport 4 - 225/45R17",
            "Michelin Pilot Sport 4 - 245/40R18",
            "Goodyear Eagle F1 - 205/55R16",
            "Pirelli Cinturato P7 - 225/50R17",
            "Bridgestone Turanza - 195/65R15"
        };
        // =========================================================================
        // MOTOR DE PRECIOS CENTRAL (NUEVO)
        // =========================================================================
        private decimal ObtenerPrecioArticulo(string descripcion)
        {
            // 👉 PARA EL EQUIPO DE INTEGRACIÓN (2/3): 
            // Este switch está "quemado" en el código para la maqueta. 
            // En producción, deben cambiar esta función para que reciba el nombre del item 
            // y haga una consulta a SQL (ej. SELECT PrecioVenta FROM Producto WHERE...)
            // y retorne el valor real de la base de datos.

            switch (descripcion)
            {
                case "Michelin Pilot Sport 4 - 225/45R17": return 185.00m;
                case "Michelin Pilot Sport 4 - 245/40R18": return 210.00m;
                case "Goodyear Eagle F1 - 205/55R16": return 160.00m;
                case "Pirelli Cinturato P7 - 225/50R17": return 145.00m;
                case "Bridgestone Turanza - 195/65R15": return 130.00m;
                case "Alineación Computarizada": return 45.00m;
                case "Balanceo por Goma": return 15.00m;
                default: return 0m;
            }
        }
        public void ActualizarGaveta()
        {
            decimal totalEnGaveta = FondoCaja + TotalEfectivoDelDia;
            lblDineroCaja.Text = "Dinero en Gaveta: " + totalEnGaveta.ToString("C");
        }
        private void GuardarVentaOffline()
        {
            // 👉 PARA EL EQUIPO DE INTEGRACIÓN (3/3): 
            // Actualmente este método guarda un archivo .txt como backup local.
            // Aquí es donde deben poner el código de conexión a SQL para hacer 
            // el INSERT INTO Factura y el INSERT INTO DetalleFactura.
            // Mantengan el código del .txt dentro de un bloque 'catch' por si la BD se cae.

            try
            {
                string fechaHora = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
                string textoFactura = $"FECHA: {fechaHora}\n";
                textoFactura += $"TOTAL PAGADO: {lblTotal.Text} (Pago en {reciboMetodo})\n";
                textoFactura += "ARTÍCULOS VENDIDOS:\n";

                foreach (DataGridViewRow fila in dgvCarrito.Rows)
                {
                    string cantidad = fila.Cells[2].Value.ToString();
                    string descripcion = fila.Cells[1].Value.ToString();
                    string subtotal = fila.Cells[4].Value.ToString();
                    textoFactura += $"  - {cantidad}x {descripcion} (${subtotal})\n";
                }

                textoFactura += "--------------------------------------------------\n";
                System.IO.File.AppendAllText("Backup_VentasOffline.txt", textoFactura);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Advertencia: No se pudo guardar el respaldo offline. " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            ActualizarGaveta();
            cmbCliente.Items.Add("Cliente Genérico (Al Contado)");
            cmbCliente.Items.Add("Juan Pérez");
            cmbCliente.Items.Add("María Gómez");
            cmbCliente.SelectedIndex = 0;

            cmbSucursal.Items.Add("Principal - Santo Domingo");
            cmbSucursal.Items.Add("Sucursal Santiago");
            cmbSucursal.SelectedIndex = 0;

            cmbEmpleado.Items.Add("Carlos Cajero");
            cmbEmpleado.Items.Add("Ana Ventas");
            cmbEmpleado.SelectedIndex = 0;

            cmbTipoItem.Items.Add("Producto (Goma)");
            cmbTipoItem.Items.Add("Servicio (Taller)");
            cmbTipoItem.SelectedIndex = 0;
        }
        private void cmbTipoItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbBuscarItem.Items.Clear();

            if (cmbTipoItem.Text == "Producto (Goma)")
            {
                txtBuscarMedida.Enabled = true;
                foreach (string goma in inventarioGomas)
                {
                    cmbBuscarItem.Items.Add(goma);
                }
            }
            else if (cmbTipoItem.Text == "Servicio (Taller)")
            {
                txtBuscarMedida.Enabled = false;
                txtBuscarMedida.Clear();
                cmbBuscarItem.Items.Add("Alineación Computarizada");
                cmbBuscarItem.Items.Add("Balanceo por Goma");
            }

            if (cmbBuscarItem.Items.Count > 0) cmbBuscarItem.SelectedIndex = 0;
        }
        // =========================================================================
        // MOSTRAR PRECIO EN VIVO (NUEVO)
        // =========================================================================
        private void txtBuscarMedida_TextChanged(object sender, EventArgs e)
        {
            // EL GUARDIÁN REPARADO
            if (cmbTipoItem.Text != "Producto (Goma)") return;

            cmbBuscarItem.Items.Clear();
            string filtro = txtBuscarMedida.Text.ToLower();

            foreach (string goma in inventarioGomas)
            {
                if (goma.ToLower().Contains(filtro))
                {
                    cmbBuscarItem.Items.Add(goma);
                }
            }

            if (cmbBuscarItem.Items.Count > 0) cmbBuscarItem.SelectedIndex = 0;
        }
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cmbBuscarItem.Text))
            {
                MessageBox.Show("Por favor, seleccione un artículo.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string tipo = cmbTipoItem.Text;
            string descripcion = cmbBuscarItem.Text;
            int cantidad = (int)nudCantidad.Value;

            // Usamos el Motor de Precios que creamos arriba
            decimal precioUnitario = ObtenerPrecioArticulo(descripcion);

            if (precioUnitario == 0m)
            {
                MessageBox.Show("Este artículo no tiene un precio válido o no hay stock.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            decimal subTotalLinea = cantidad * precioUnitario;
            dgvCarrito.Rows.Add(tipo, descripcion, cantidad, precioUnitario, subTotalLinea);

            decimal sumaSubTotal = 0;
            foreach (DataGridViewRow fila in dgvCarrito.Rows)
            {
                sumaSubTotal += Convert.ToDecimal(fila.Cells[4].Value);
            }

            decimal impuesto = sumaSubTotal * 0.18m;
            decimal totalGeneral = sumaSubTotal + impuesto;

            lblSubTotal.Text = sumaSubTotal.ToString("C");
            lblImpuesto.Text = impuesto.ToString("C");
            lblTotal.Text = totalGeneral.ToString("C");
        }
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvCarrito.CurrentRow != null)
            {
                dgvCarrito.Rows.Remove(dgvCarrito.CurrentRow);
                decimal sumaSubTotal = 0;
                foreach (DataGridViewRow fila in dgvCarrito.Rows)
                {
                    sumaSubTotal += Convert.ToDecimal(fila.Cells[4].Value);
                }
                decimal impuesto = sumaSubTotal * 0.18m;
                decimal totalGeneral = sumaSubTotal + impuesto;

                lblSubTotal.Text = sumaSubTotal.ToString("C");
                lblImpuesto.Text = impuesto.ToString("C");
                lblTotal.Text = totalGeneral.ToString("C");
            }
            else
            {
                MessageBox.Show("Seleccione el artículo que desea eliminar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void btnFacturar_Click(object sender, EventArgs e)
        {
            if (dgvCarrito.Rows.Count == 0)
            {
                MessageBox.Show("El carrito está vacío.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal sumaSubTotal = 0;
            foreach (DataGridViewRow fila in dgvCarrito.Rows)
            {
                sumaSubTotal += Convert.ToDecimal(fila.Cells[4].Value);
            }
            decimal totalFactura = sumaSubTotal + (sumaSubTotal * 0.18m);

            FormCobro pantallaCobro = new FormCobro();
            pantallaCobro.TotalPagar = totalFactura;
            pantallaCobro.EfectivoEnCaja = FondoCaja + TotalEfectivoDelDia;

            if (pantallaCobro.ShowDialog() == DialogResult.OK)
            {
                reciboMetodo = pantallaCobro.MetodoPago;
                reciboEfectivo = pantallaCobro.EfectivoEntregado;
                reciboDevuelta = pantallaCobro.CambioDevuelto;

                if (reciboMetodo == "Efectivo")
                {
                    TotalEfectivoDelDia += (reciboEfectivo - reciboDevuelta);
                    ActualizarGaveta();
                }

                MessageBox.Show("¡El pago fue recibido con éxito!", "Venta Completada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                GuardarVentaOffline();
                previewImprimir.ShowDialog();

                dgvCarrito.Rows.Clear();
                lblSubTotal.Text = "$0.00";
                lblImpuesto.Text = "$0.00";
                lblTotal.Text = "$0.00";
                nudCantidad.Value = 1;
                cmbBuscarItem.Text = "";
                lblPrecioVista.Text = "Precio: $0.00"; // Limpiamos la etiqueta del precio también
            }
        }
        private void docImprimir_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Font fuenteTitulo = new Font("Arial", 14, FontStyle.Bold);
            Font fuenteNormal = new Font("Arial", 10);
            int y = 20;

            e.Graphics.DrawString("PRECISION TIRE", fuenteTitulo, Brushes.Black, new PointF(80, y));
            y += 30;
            e.Graphics.DrawString("Recibo de Venta", fuenteNormal, Brushes.Black, new PointF(100, y));
            y += 30;
            e.Graphics.DrawString($"Cliente: {cmbCliente.Text}", fuenteNormal, Brushes.Black, new PointF(10, y));
            y += 20;
            e.Graphics.DrawString($"Atendido por: {cmbEmpleado.Text}", fuenteNormal, Brushes.Black, new PointF(10, y));
            y += 20;
            e.Graphics.DrawString($"Sucursal: {cmbSucursal.Text}", fuenteNormal, Brushes.Black, new PointF(10, y));
            y += 25;
            // --------------------------------------------------------------------------

            e.Graphics.DrawString("-------------------------------------------------", fuenteNormal, Brushes.Black, new PointF(10, y));
            y += 20;

            e.Graphics.DrawString("-------------------------------------------------", fuenteNormal, Brushes.Black, new PointF(10, y));
            y += 20;

            foreach (DataGridViewRow fila in dgvCarrito.Rows)
            {
                string descripcion = fila.Cells[1].Value.ToString();
                string cantidad = fila.Cells[2].Value.ToString();
                string subTotalFila = fila.Cells[4].Value.ToString();

                e.Graphics.DrawString($"{cantidad}x {descripcion}", fuenteNormal, Brushes.Black, new PointF(10, y));
                e.Graphics.DrawString($"${subTotalFila}", fuenteNormal, Brushes.Black, new PointF(250, y));
                y += 25;
            }

            e.Graphics.DrawString("-------------------------------------------------", fuenteNormal, Brushes.Black, new PointF(10, y));
            y += 20;
            e.Graphics.DrawString($"SubTotal: {lblSubTotal.Text}", fuenteNormal, Brushes.Black, new PointF(150, y));
            y += 20;
            e.Graphics.DrawString($"Impuesto: {lblImpuesto.Text}", fuenteNormal, Brushes.Black, new PointF(150, y));
            y += 25;
            e.Graphics.DrawString($"TOTAL: {lblTotal.Text}", fuenteTitulo, Brushes.Black, new PointF(120, y));
            y += 30;
            e.Graphics.DrawString("-------------------------------------------------", fuenteNormal, Brushes.Black, new PointF(10, y));
            y += 20;

            e.Graphics.DrawString($"Pago en: {reciboMetodo}", fuenteNormal, Brushes.Black, new PointF(10, y));
            y += 20;

            if (reciboMetodo == "Tarjeta")
            {
                e.Graphics.DrawString($"Monto Cobrado: {reciboEfectivo.ToString("C")}", fuenteNormal, Brushes.Black, new PointF(10, y));
                y += 20;
            }
            else
            {
                e.Graphics.DrawString($"Recibido: {reciboEfectivo.ToString("C")}", fuenteNormal, Brushes.Black, new PointF(10, y));
                y += 20;
                e.Graphics.DrawString($"Devuelta: {reciboDevuelta.ToString("C")}", fuenteNormal, Brushes.Black, new PointF(10, y));
                y += 20;
            }

            e.Graphics.DrawString("-------------------------------------------------", fuenteNormal, Brushes.Black, new PointF(10, y));
            y += 20;
            e.Graphics.DrawString("¡Gracias por preferir Precision Tire!", fuenteNormal, Brushes.Black, new PointF(50, y));
        }
        private void btnCierreCaja_Click(object sender, EventArgs e)
        {
            decimal totalEnGaveta = FondoCaja + TotalEfectivoDelDia;
            string reporte = "=== REPORTE DE CIERRE DE CAJA ===\n\n" +
                             $"Fondo Inicial (Mañana): {FondoCaja.ToString("C")}\n" +
                             $"Ventas en Efectivo: {TotalEfectivoDelDia.ToString("C")}\n" +
                             "--------------------------------------------------\n" +
                             $"DINERO TOTAL EN GAVETA: {totalEnGaveta.ToString("C")}\n\n" +
                             "¿Desea cerrar la caja y salir del sistema?";

            DialogResult respuesta = MessageBox.Show(reporte, "Cuadre de Turno", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (respuesta == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
        private void cmbBuscarItem_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            // Cada vez que el cajero toca un artículo, buscamos su precio y lo mostramos
            if (!string.IsNullOrWhiteSpace(cmbBuscarItem.Text))
            {
                decimal precioVista = ObtenerPrecioArticulo(cmbBuscarItem.Text);
                lblPrecioVista.Text = "Precio: " + precioVista.ToString("C");
            }
        }
        private void btnAnularVenta_Click(object sender, EventArgs e)
        {
            // Confirmamos por si el cajero le dio por accidente
            DialogResult respuesta = MessageBox.Show("¿Está seguro que desea cancelar toda la venta actual?", "Anular Venta", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (respuesta == DialogResult.Yes)
            {
                dgvCarrito.Rows.Clear();
                lblSubTotal.Text = "$0.00";
                lblImpuesto.Text = "$0.00";
                lblTotal.Text = "$0.00";
                nudCantidad.Value = 1;
                cmbBuscarItem.SelectedIndex = -1; // Deselecciona el artículo
                lblPrecioVista.Text = "Precio: $0.00";
            }
        }
    }
}