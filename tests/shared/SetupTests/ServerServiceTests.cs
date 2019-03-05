using System;
using System.Collections.Generic;
using System.Text;
using ServerService = SuperFastDB.ServerService;

namespace SetupTests
{
    public static class ServerServiceTests
    {

        public static void InstallServerService()
        {
            // Instala o Servidor
            ServerService.Installer.Install(
                SuperFastDB_Server.Helper.GetExecutingAssemblyLocation() // Obtém o caminho do executável
                );
        }

        public static void UninstallServerService()
        {
            // Desinstala o Servidor
            ServerService.Installer.Uninstall(
                SuperFastDB_Server.Helper.GetExecutingAssemblyLocation() // Obtém o caminho do executável
                );
        }
    }
}
