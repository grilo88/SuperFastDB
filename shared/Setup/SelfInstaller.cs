using System;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Reflection;
using System.Text;

namespace SuperFastDB
{
    public static class SelfInstaller
    {
        /// <summary> Instala o serviço. </summary>
        /// <param name="exePath">Caminho do arquivo executável</param>
        public static void Install(string exePath)
        {
            ManagedInstallerClass.InstallHelper(new string[] { exePath });
        }

        /// <summary> Desinstala o serviço. </summary>
        /// <param name="exePath">Caminho do arquivo executável</param>
        public static void Uninstall(string exePath)
        {
            ManagedInstallerClass.InstallHelper(new string[] { "/u", exePath });
        }
    }
}
