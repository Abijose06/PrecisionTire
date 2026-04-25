using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Core.DataEntry
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Program.cs
            FormLogin login = new FormLogin();

            if (login.ShowDialog() != DialogResult.OK)
                return;

            if (login.RolUsuario != "Administrador" && login.RolUsuario != "Cajero" && login.RolUsuario != "Cliente")
                return;

            Application.Run(new frmPrincipal(login.RolUsuario));
        }
    }
}
