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

            // Mostrar login primero
            FormLogin login = new FormLogin();
            if (login.ShowDialog() != DialogResult.OK)
                return;

            Application.Run(new frmPrincipal());
        }
    }
}
