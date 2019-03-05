using System;
using System.Collections.Generic;
using System.Text;

namespace SuperFastDB.Server
{
    public static class CoreProgram
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        public static void Main(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i];

                // TODO: Argumentos
            }

            Instance server = new Instance();
            server.Console();
        }
    }
}
