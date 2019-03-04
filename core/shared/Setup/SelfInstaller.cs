using System;
using System.Collections.Generic;

#if x86
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
#if x86
            ManagedInstallerClass.InstallHelper(new string[] { exePath });
#else
            throw new NotImplementedException();
#endif
        }

        /// <summary> Desinstala o serviço. </summary>
        /// <param name="exePath">Caminho do arquivo executável</param>
        public static void Uninstall(string exePath)
        {
#if x86
            ManagedInstallerClass.InstallHelper(new string[] { "/u", exePath });
#else
            throw new NotImplementedException();
#endif
        }
    }
}
