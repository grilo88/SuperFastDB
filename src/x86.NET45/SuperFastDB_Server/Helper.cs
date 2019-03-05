using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SuperFastDB_Server
{
    public static class Helper
    {

        /// <summary>
        /// Obtém o caminho do executável deste assembly
        /// </summary>
        /// <returns></returns>
        public static string GetExecutingAssemblyLocation()
        {
            return Assembly.GetExecutingAssembly().Location;
        }
    }
}
