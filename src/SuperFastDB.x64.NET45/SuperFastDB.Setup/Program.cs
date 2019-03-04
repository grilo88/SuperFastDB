using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

// Neste caso estamos usando x86 só para o uso do forms
using setupforms = setup_forms_x64_net45;

namespace SuperFastDB_Setup
{
    public static class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new setupforms.frmMain());
        }
    }
}
