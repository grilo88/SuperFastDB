using SuperFastDB.ServerService;
using System.ComponentModel;

namespace SuperFastDB_Server
{
    [RunInstaller(true)]
    public class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller() => this.Installers.AddRange(new Installer().Installers());
    }

    static class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        static void Main(string[] args) => Service.Main(args);
    }
}
