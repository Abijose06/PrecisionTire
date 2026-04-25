using System;
using System.Windows.Forms;

namespace Core.DataEntry
{
    public partial class frmPrincipal : Form
    {
        private readonly string _rol;

        public frmPrincipal(string rol)
        {
            InitializeComponent();
            _rol = rol;

            if (_rol == "Cliente")
            {
                // Solo Producto y Servicio visibles
                clienteToolStripMenuItem.Visible = false;
                empleadoToolStripMenuItem.Visible = false;
                surcursalToolStripMenuItem.Visible = false;
                vehiculoToolStripMenuItem.Visible = false;
            }
            else if (_rol == "Cajero")
            {
                // Ve todo, no se oculta nada
            }
            // Administrador: ve todo por defecto
        }

        // 1. Botón del menú para Empleados
        private void empleadoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmEmpleado frm = new frmEmpleado();
            frm.MdiParent = this; 
            frm.Show(); 
        }

        // 2. Botón del menú para Clientes
        private void clienteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCliente frm = new frmCliente(_rol);
            frm.MdiParent = this;
            frm.Show();
        }

        // 3. Botón del menú para Sucursales
        private void sucursalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSucursal frm = new frmSucursal();
            frm.MdiParent = this;
            frm.Show();
        }

        // 4. Botón del menú para Productos
        private void productoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form formHijo in this.MdiChildren)
            {
                if (formHijo.GetType() == typeof(frmProducto))
                {
                    formHijo.Focus();
                    return;
                }
            }

            frmProducto frm = new frmProducto(_rol); // ← pasar rol
            frm.MdiParent = this;
            frm.Show();
        }

        private void servicioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmServicio frm = new frmServicio(_rol); // ← pasar rol
            frm.MdiParent = this;
            frm.Show();
        }

        // 6. Botón del menú para Vehículos
        private void vehiculoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmVehiculo frm = new frmVehiculo();
            frm.MdiParent = this;
            frm.Show();
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit(); // Cierra el programa por completo
        }
    }
}