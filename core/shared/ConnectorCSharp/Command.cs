using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace SuperFastDB.Data
{
    public class FastDBCommand : IDbCommand
    {
        public string CommandText { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int CommandTimeout { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public CommandType CommandType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IDbConnection Connection { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IDataParameterCollection Parameters => throw new NotImplementedException();

        public IDbTransaction Transaction { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public UpdateRowSource UpdatedRowSource { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public FastDBCommand(string cmdText)
        {
            throw new NotImplementedException();
        }

        public FastDBCommand(string cmdText, FastDBConnection connection)
        {
            throw new NotImplementedException();
        }

        public FastDBCommand(string cmdText, FastDBConnection connection, FastDBTransaction transaction)
        {
            throw new NotImplementedException();
        }

        public void Cancel()
        {
            throw new NotImplementedException();
        }

        public IDbDataParameter CreateParameter()
        {
            throw new NotImplementedException();
        }

        public int ExecuteNonQuery()
        {
            throw new NotImplementedException();
        }

        public IDataReader ExecuteReader()
        {
            throw new NotImplementedException();
        }

        public IDataReader ExecuteReader(CommandBehavior behavior)
        {
            throw new NotImplementedException();
        }

        public object ExecuteScalar()
        {
            throw new NotImplementedException();
        }

        public void Prepare()
        {
            throw new NotImplementedException();
        }

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
        // ~FastDBCommand() {
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
