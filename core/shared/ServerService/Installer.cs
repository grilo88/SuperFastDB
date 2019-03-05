using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace SuperFastDB.ServerService
{
    public class Installer
    {
        private ServiceProcessInstaller serviceProcessInstaller;
        private ServiceInstaller serviceInstaller;

        public Installer()
        {
            this.serviceProcessInstaller = new ServiceProcessInstaller();
            this.serviceInstaller = new ServiceInstaller();

            this.serviceProcessInstaller.Account = ServiceAccount.LocalSystem;
            this.serviceProcessInstaller.Password = null;
            this.serviceProcessInstaller.Username = null;

            // TODO: Implementar configurações de instalação do serviço


            this.serviceInstaller.ServiceName = "SuperFastDB Server";

#if x86
            this.serviceInstaller.DisplayName = "SuperFastDB Server x86";
            
#elif x64
            this.serviceInstaller.DisplayName = "SuperFastDB Server x64";
#endif

            this.serviceInstaller.StartType = ServiceStartMode.Automatic;
        }

        public System.Configuration.Install.Installer[] Installers()
        {
            return new System.Configuration.Install.Installer[]
            {
                this.serviceProcessInstaller,
                this.serviceInstaller
            };
        }

        /// <summary>
        /// Instala o Serviço
        /// </summary>
        /// <param name="exePath">Caminho do arquivo executável</param>
        public static void Install(string exePath)
        {
            ManagedInstallerClass.InstallHelper(new string[] { exePath });
        }

        /// <summary>
        /// Desinstala o Serviço
        /// </summary>
        /// <param name="exePath">Caminho do arquivo executável</param>
        public static void Uninstall(string exePath)
        {
            ManagedInstallerClass.InstallHelper(new string[] { "/u", exePath });
        }

        public static void Install()
        {
            string exePath = "";
#if x64
            exePath = @"..\..\..\TesteServico_x64\bin\Debug\TesteServico.exe";
#elif x86
            exePath = @"..\..\TesteServico_x86\bin\Debug\TesteServico.exe";
#endif

            if (!File.Exists(exePath))
            {
                throw new FileNotFoundException(exePath);
            }

            Install(exePath);
        }

        public static void Uninstall()
        {
            string exePath = "";
#if x64
            exePath = @"..\..\..\TesteServico_x64\bin\Debug\TesteServico.exe";
#elif x86
            exePath = @"..\..\..\TesteServico_x86\bin\Debug\TesteServico.exe";
#endif

            if (!File.Exists(exePath))
            {
                throw new FileNotFoundException(exePath);
            }

            Uninstall(exePath);
        }
    }
}
