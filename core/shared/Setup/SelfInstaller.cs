using System;
using System.Collections.Generic;
#if NET45
using System.Configuration.Install;
#endif
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
#if NET45
            ManagedInstallerClass.InstallHelper(new string[] { exePath });
#else
            throw new NotImplementedException();
#endif
        }

        /// <summary> Desinstala o serviço. </summary>
        /// <param name="exePath">Caminho do arquivo executável</param>
        public static void Uninstall(string exePath)
        {
#if NET45
            ManagedInstallerClass.InstallHelper(new string[] { "/u", exePath });
#else
            throw new NotImplementedException();
#endif
        }
    }
}
