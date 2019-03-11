#define UNSAFE

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InterpretadorSQL
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int tick = Environment.TickCount;

            string sql = @"select (Select 2 + (select 2 as ret) as teste ) + 4 + (select 1), (Select 2 as teste) as top FROM world.city where id > 0 + 1 * 4 - 10";
            for (int i = 0; i < 10; i++)
            {
                Query(sql);
            }

            int tempoGasto = Environment.TickCount - tick;

            this.Text = tempoGasto.ToString() + "ms";
        }

        

        #region Constant Tokens
        const uint SELECT = 1564;
        const uint FROM = 779;
        const uint WHERE = 5377;
        const uint EQUAL_OPERATOR = 61;
        const uint ADD_OPERATOR = 43;
        const uint MULTIPLY_OPERATOR = 42;
        const uint ALIAS = 0;
        #endregion

        Stack<object> exp = new Stack<object>();

        private object Query(string commandText, bool sub_select = false)
        {
            // SELECT [Expressão],[Expressão],[Expressão] ... FROM [Banco].[Tabela] WHERE [Expressão]

            // ##########################
            //  Programação Estruturada
            // ##########################
            int seq_l = 0;      // Sequência Lógica

            #region Sql
            char[] sql = commandText.ToCharArray();
            char[] sql_u = commandText.ToUpper().ToCharArray();
            int sql_length = sql.Length;
            #endregion

            #region Sequência Léxica
            int n_pos = -1;     // Novo Pos
            int n_pos2 = -1;    // Novo Pos2
            bool exp = false;   // Expressão
            #endregion

            uint str_token_pos = 0;
            uint str_token_hashcode = 0;

            #region String
            char[] str_string = new char[2048];
            bool b_string = false;
            int _start_string = 0;
            #endregion

            //unsafe
            //{
            //    fixed (char* f_sql = _sql)
            //    fixed (char* f_sql_u = _sql_u)
            //    fixed (char* f_str_t = _str_t)
            //    {
            for (int pos = 0; pos < sql_length; pos++)
            {
                Next:

                if (/* SELECT */ seq_l == 1)
                {
                    pos = NextIndexDiffSpace(sql, pos);
                    n_pos = IndexEndCol(sql_u, pos);

                    #region Sub-Select
                    if (sql[pos] == '(' && (n_pos2 = NextCloseParenthesis(
                        sql, pos + 1 /*pos = '('*/, n_pos - 1 /*n_pos = ','*/)) > -1) // Detecta se estamos entre parênteses
                    {
                        int hash = -1;
                        Hashcode_token(sql_u, NextIndexDiffSpace(sql, pos + 1), out hash);

                        exp = hash != SELECT; // Detecta se este é uma expressão ou subselect

                        if (!exp) // SubSelect
                        {
                            pos++;
                            object sub_sel = Query(new string(sql, pos, n_pos2 - pos), true);
                            n_pos = ++n_pos2;
                        }
                    }
                    #endregion

                    #region Expressão
                    if (exp) // Expressão
                    {
                        object expression = Query(new string(sql, ++pos, n_pos - pos), false);
                        exp = false;
                    }
                    #endregion

                    n_pos = NextIndexDiffSpace(sql, pos);

                    if (sql[n_pos] == ',')
                    {
                        pos = n_pos + 1;
                        goto Next; // Próxima coluna
                    }
                    else if ( // Alias
                        sql[n_pos + 0] == 'A' &&
                        sql[n_pos + 1] == 'S' &&
                        sql[n_pos + 2] == ' ')
                    {

                    }
                }
                else if (b_string) // Obtém a string
                {
                    if (sql[pos] == '\\')
                    {
                        if (sql[pos + 1] == '\'') // Ignora \'
                        {
                            pos++;
                            continue;
                        }
                    }
                    else if (sql[pos] == '\'')
                    {
                        // Empilha a string
                        //exp.Push(new string(_sql, ++_start_string, pos - _start_string));
                        b_string = false;
                    }
                    continue;
                }
                else
                {
                    if (sql[pos] == '\'')
                    {
                        b_string = true;
                        _start_string = pos;
                        continue;
                    }
                    #region Token
                    if (sql[pos] == '(' || sql[pos] == ')')
                    {
                        str_token_pos = 0;
                        str_token_hashcode = 0;
                    }
                    else if (sql[pos] == ' ')
                    {
                        if (str_token_hashcode == 0)
                        {
                            pos = NextIndexDiffSpace(sql, pos) - 1;
                            continue; // Pula espaços em branco
                        }

                        #region Analisador léxico

                        if (str_token_hashcode == SELECT) // Instrução SELECT
                        {
                            seq_l = 1;
                        }
                        #endregion

                        //exp.Push(_str_token_hashcode); // Lança o token na pilha
                        str_token_pos = 0;
                        str_token_hashcode = 0;
                    }
                    else
                    {
                        str_token_hashcode += (++str_token_pos) * sql_u[pos];
                    }
                    #endregion
                }
            }
            //    }
            //}

            return null;
        }

        private int NextCloseParenthesis(char[] sql, int pos, int max_pos)
        {
            int nivel = 0;

            for (int i = pos; i < max_pos + 1; i++)
            {
                if (sql[i] == '(')
                {
                    nivel++;
                }
                else if (sql[i] == ')')
                {
                    nivel--;

                    if (nivel == -1) return i;
                }
            }

            return -1;
        }
        

        /// <summary>
        /// Obtém o token da palavra
        /// </summary>
        /// <param name="sql_u"></param>
        /// <param name="pos"></param>
        /// <param name="hashcode"></param>
        /// <returns>Retorna o novo pos</returns>
        private int Hashcode_token(char[] sql_u, int pos, out int hashcode)
        {
            int str_token_hashcode = 0;
            int str_token_pos = 0;
            hashcode = -1;

            for (int i = pos; i < sql_u.Length; i++)
            {
                if (sql_u[i] == ' ')
                {
                    hashcode = str_token_hashcode;
                    return i;
                }
                else
                {
                    str_token_hashcode += (++str_token_pos) * sql_u[i];
                }
            }

            return sql_u.Length - 1;
        }

        /// <summary>
        /// Retorna o índice do final da coluna
        /// </summary>
        /// <param name="sql_u"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        private int IndexEndCol(char[] sql_u, int pos)
        {
            bool b_string = false;
            int start_string = -1;
            bool ret_space = false;

            for (int i = pos; i < sql_u.Length; i++)
            {
                if (b_string) {
                    if (sql_u[i] == '\\') {
                        if (sql_u[i + 1] == '\'') { // Ignora \'
                            i++;
                            continue;
                        }
                    }
                    else if (sql_u[i] == '\'') {
                        b_string = false;
                    }
                    continue;
                }
                else
                {
                    if (sql_u[i] == '\'') {
                        b_string = true;
                        start_string = i;
                        continue;
                    }
                    else if (sql_u[i] == ' ') {
                        if (ret_space)
                            return i;
                        else
                            continue;
                    }
                    else if (sql_u[i] == ',') {
                        return i;
                    }
                    else if ( // Alias
                        sql_u[i + 0] == 'A' &&
                        sql_u[i + 1] == 'S' &&
                        sql_u[i + 2] == ' ') {
                        i = NextIndexDiffSpace(sql_u, i + 3);
                        ret_space = true;
                    }
                }
            }

            return sql_u.Length - 1;
        }

        /// <summary>
        /// Obtém o índice do próximo caractere diferente de espaço
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        private int NextIndexDiffSpace(char[] sql, int pos)
        {
            for (int i = pos; i < sql.Length; i++) {
                if (sql[i] == ' ')
                    continue;
                else
                    return i;
            }
            return -1;
        }

        private void btnSolicitacaoSQL_Click(object sender, EventArgs e)
        {
            SolicitacaoSQL();
        }

        private void SolicitacaoSQL()
        {
            RPN rpn = new RPN();

            string commandText = txtSql.Text;
            char[] infix_ = commandText.ToCharArray();
            char[] infix_u_ = commandText.ToUpper().ToCharArray();

            int iteracoes = Convert.ToInt32(txtIteracoes.Text);

            int tick = Environment.TickCount;
            object result = null;
            unsafe
            {
                fixed (char* infix = infix_)
                fixed (char* infix_u = infix_u_)
                {
                    Queue<char[]> strings;
                    char[] pos_fixa = null;
                    RPN.variavel[] variaveis = null;

                    for (int i = 0; i < iteracoes; i++)
                    {
                        pos_fixa = rpn.InfixaParaPosfixa(infix_, infix_u_, out strings);
                        result = rpn.Avaliar(pos_fixa, strings, variaveis, out variaveis);
                    }

                    txtPosFixa.Text = new string(pos_fixa);

                }
            }
            int tempoGasto = Environment.TickCount - tick;
            this.Text = tempoGasto.ToString() + "ms";

            txtResultado.Text = Convert.ToString(result);
        }
    }
}
