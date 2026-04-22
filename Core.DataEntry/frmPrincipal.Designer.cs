namespace Core.DataEntry
{
    partial class frmPrincipal
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.inserciónToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sistemaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.salirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clienteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.empleadoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.surcursalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.productoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.categoriaProductoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.servicioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vehiculoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vehiculoClienteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.inserciónToolStripMenuItem,
            this.sistemaToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // inserciónToolStripMenuItem
            // 
            this.inserciónToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clienteToolStripMenuItem,
            this.empleadoToolStripMenuItem,
            this.surcursalToolStripMenuItem,
            this.productoToolStripMenuItem,
            this.categoriaProductoToolStripMenuItem,
            this.servicioToolStripMenuItem,
            this.vehiculoToolStripMenuItem,
            this.vehiculoClienteToolStripMenuItem});
            this.inserciónToolStripMenuItem.Name = "inserciónToolStripMenuItem";
            this.inserciónToolStripMenuItem.Size = new System.Drawing.Size(67, 20);
            this.inserciónToolStripMenuItem.Text = "Inserción";
            // 
            // sistemaToolStripMenuItem
            // 
            this.sistemaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.salirToolStripMenuItem});
            this.sistemaToolStripMenuItem.Name = "sistemaToolStripMenuItem";
            this.sistemaToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.sistemaToolStripMenuItem.Text = "Sistema";
            // 
            // salirToolStripMenuItem
            // 
            this.salirToolStripMenuItem.Name = "salirToolStripMenuItem";
            this.salirToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.salirToolStripMenuItem.Text = "Salir";
            // 
            // clienteToolStripMenuItem
            // 
            this.clienteToolStripMenuItem.Name = "clienteToolStripMenuItem";
            this.clienteToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.clienteToolStripMenuItem.Text = "Cliente";
            // 
            // empleadoToolStripMenuItem
            // 
            this.empleadoToolStripMenuItem.Name = "empleadoToolStripMenuItem";
            this.empleadoToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.empleadoToolStripMenuItem.Text = "Empleado";
            // 
            // surcursalToolStripMenuItem
            // 
            this.surcursalToolStripMenuItem.Name = "surcursalToolStripMenuItem";
            this.surcursalToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.surcursalToolStripMenuItem.Text = "Surcursal";
            // 
            // productoToolStripMenuItem
            // 
            this.productoToolStripMenuItem.Name = "productoToolStripMenuItem";
            this.productoToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.productoToolStripMenuItem.Text = "Producto";
            // 
            // categoriaProductoToolStripMenuItem
            // 
            this.categoriaProductoToolStripMenuItem.Name = "categoriaProductoToolStripMenuItem";
            this.categoriaProductoToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.categoriaProductoToolStripMenuItem.Text = "CategoriaProducto";
            // 
            // servicioToolStripMenuItem
            // 
            this.servicioToolStripMenuItem.Name = "servicioToolStripMenuItem";
            this.servicioToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.servicioToolStripMenuItem.Text = "Servicio";
            // 
            // vehiculoToolStripMenuItem
            // 
            this.vehiculoToolStripMenuItem.Name = "vehiculoToolStripMenuItem";
            this.vehiculoToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.vehiculoToolStripMenuItem.Text = "Vehiculo";
            // 
            // vehiculoClienteToolStripMenuItem
            // 
            this.vehiculoClienteToolStripMenuItem.Name = "vehiculoClienteToolStripMenuItem";
            this.vehiculoClienteToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.vehiculoClienteToolStripMenuItem.Text = "Vehiculo-Cliente";
            // 
            // FormPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormPrincipal";
            this.Text = "Panel de Administración CORE - PrecisionTire";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem inserciónToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clienteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem empleadoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem surcursalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem productoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem categoriaProductoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem servicioToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vehiculoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vehiculoClienteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sistemaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem salirToolStripMenuItem;
    }
}

