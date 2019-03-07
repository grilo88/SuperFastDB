using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperFast
{
    public class SqlQueryException : Exception
    {
        public int pos;

        public SqlQueryException()
        {
        }

        public SqlQueryException(string message, int pos)
            : base(message)
        {
            this.pos = pos;
        }
        public SqlQueryException(string message, Exception inner)
            : base(message, inner)
        {
        }

        
    }
}
