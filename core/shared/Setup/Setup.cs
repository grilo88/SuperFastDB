using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace SuperFastDB
{
    public class Setup : IDisposable
    {
        public Setup()
        {
        }

        /// <summary>
        /// Checa se está rodando como Administrador
        /// </summary>
        /// <returns></returns>
        public static bool IsAdministrator()
        {
#if NET45
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
#else
            throw new NotImplementedException();
#endif
        }

        /// <summary>
        /// Instala o serviço.
        /// </summary>
        /// <param name="exePath">Caminho do arquivo executável</param>
        public void InstalarServico(string exePath)
        {
            SelfInstaller.Install(exePath);
        }

        /// <summary>
        /// Desinstala o serviço.
        /// </summary>
        /// <param name="exePath">Caminho do arquivo executável</param>
        public void DesinstalarServico(string exePath)
        {
            SelfInstaller.Uninstall(exePath);
        }

        // TODO: ?

#region IDisposable Support
        private bool disposedValue = false; // Para detectar chamadas redundantes

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: descartar estado gerenciado (objetos gerenciados).
                }

                // TODO: liberar recursos não gerenciados (objetos não gerenciados) e substituir um finalizador abaixo.
                // TODO: definir campos grandes como nulos.

                disposedValue = true;
            }
        }

        // TODO: substituir um finalizador somente se Dispose(bool disposing) acima tiver o código para liberar recursos não gerenciados.
        // ~Setup() {
        //   // Não altere este código. Coloque o código de limpeza em Dispose(bool disposing) acima.
        //   Dispose(false);
        // }

        // Código adicionado para implementar corretamente o padrão descartável.
        public void Dispose()
        {
            // Não altere este código. Coloque o código de limpeza em Dispose(bool disposing) acima.
            Dispose(true);
            // TODO: remover marca de comentário da linha a seguir se o finalizador for substituído acima.
            // GC.SuppressFinalize(this);
        }
#endregion
    }
}
