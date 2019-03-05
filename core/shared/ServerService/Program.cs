using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;

namespace SuperFastDB.ServerService
{
    public partial class Service
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        public static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                // TODO: Implementar inicialização por argumentos

                if (args[0] == "1")
                {
                    
                }
            }

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new Service()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
