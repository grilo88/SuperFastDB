using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace SuperFastDB.Setup
{
    public static class Helper
    {
        /// <summary>
        /// Checa se está rodando como Administrador
        /// </summary>
        /// <returns></returns>
        public static bool IsAdministrator()
        {
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }
    }
}
