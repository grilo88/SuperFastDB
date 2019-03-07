using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperFast
{
    public static class Extensoes
    {
        public static IEnumerable<string> SplitSqlQuery(this string sql, string[] separadores, StringSplitOptions splitOptions, bool ignoreStrings = true)
        {
            bool inStrAspasDupla = false;
            bool inStrAspasSimples = false;

            int idx = 0;
            for (int pos = 0; pos < sql.Length; pos++)
            {
                if (ignoreStrings)
                {
                    char ant = (pos == 0 ? '\0' : sql[pos - 1]);
                    char ch = sql[pos];

                    if (!inStrAspasSimples)
                    {
                        if (inStrAspasDupla && ch == '"' && ant != '\\')
                        {
                            inStrAspasDupla = false;
                        }
                        else if (!inStrAspasDupla && ch == '"' && ant != '\\')
                        {
                            inStrAspasDupla = true;
                        }
                    }
                    if (!inStrAspasDupla)
                    {
                        if (inStrAspasSimples && ch == '\'' && ant != '\\')
                        {
                            inStrAspasSimples = false;
                        }
                        else if (!inStrAspasSimples && ch == '\'' && ant != '\\')
                        {
                            inStrAspasSimples = true;
                        }
                    }
                }

                if (!inStrAspasDupla && !inStrAspasSimples)
                {
                    for (int b = 0; b < separadores.Length; b++)
                    {
                        string separador = separadores[b];
                        int count = separador.Length;

                        if (pos + count > sql.Length) continue;
                        string val = sql.Substring(pos, count);

                        if (val == separador)
                        {
                            if (val == "") throw new Exception("String vazia é inválida para separador");
                            if (pos - idx > 0)
                            {
                                string parte = sql.Substring(idx, pos - idx); // obtém o valor esquerdo ao delimitador
                                yield return parte;
                            }

                            if (!ignoreStrings || splitOptions != StringSplitOptions.RemoveEmptyEntries || !string.IsNullOrWhiteSpace(separador)) // Pula entradas vazias
                            {
                                // Adiciona o separador
                                yield return separador;
                            }

                            idx = pos + count;
                            pos += count - 1;
                            break;
                        }
                    }
                }

                if (pos == sql.Length - 1)
                {
                    string ultimo = sql.Substring(idx, sql.Length - idx);
                    if (!ignoreStrings || splitOptions != StringSplitOptions.RemoveEmptyEntries || !string.IsNullOrWhiteSpace(ultimo)) // Pula entradas vazias
                    {
                        yield return ultimo;
                    }
                }
            }
        }
    }
}
