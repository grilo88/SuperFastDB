using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interpretador
{
    public class ExpressaoSql
    {
        public struct variavel
        {
            public char[] nome;
            public byte tipo;
            public char[] valor_str;
            public double valor_num;
        }

        int _compRPN = 0;
        const int CHAR_SIZE = 50;
        int _capacidade_posfixa;                        // Inicialização da capacidade da Array posfixa[]

        double[] _dblArr = new double[10];
        char[,] _strArr = new char[10, CHAR_SIZE];

        public int ComprimentoRPN { set { _compRPN = value; } }

        // https://pt.cppreference.com/w/cpp/language/operator_precedence
        const char PRIO_PARENTESE = (char)0;            // (, )
        const char PRIO_FUNC = (char)2;                 // Função
        const char PRIO_NEGACAO = (char)3;              // !
        const char PRIO_POTENCIA = (char)4;             // ^
        const char PRIO_MULTIPLICACAO = (char)5;        // *
        const char PRIO_DIVISAO = (char)5;              // /
        const char PRIO_MODULO = (char)5;               // %
        const char PRIO_ADICAO = (char)6;               // +
        const char PRIO_SUBTRACAO = (char)6;            // -
        const char PRIO_MENOR_QUE = (char)8;            // <
        const char PRIO_MENOR_OU_IGUAL_A = (char)8;     // <=
        const char PRIO_MAIOR_QUE = (char)8;            // >
        const char PRIO_MAIOR_OU_IGUAL_A = (char)8;     // >=
        const char PRIO_IGUAL_A = (char)8;              // =
        const char PRIO_DIFERENTE_DE = (char)8;         // !=
        const char PRIO_AND = (char)13;                 // &
        const char PRIO_ALL = (char)20;                 // ALL
        const char PRIO_ANY = (char)20;                 // ANY
        const char PRIO_BETWEEN = (char)20;             // BETWEEN
        const char PRIO_CASE = (char)20;                // CASE
        const char PRIO_WHEN = (char)20;                // WHEN
        const char PRIO_THEN = (char)20;                // THEN
        const char PRIO_ELSE = (char)20;                // ELSE
        const char PRIO_END = (char)20;                 // END

        const char PRIO_IN = (char)20;                  // IN
        const char PRIO_LIKE = (char)20;                // LIKE
        const char PRIO_OR = (char)20;                  // OR
        const char PRIO_SOME = (char)20;                // SOME
        const char PRIO_VIRGULA = (char)100;            // VIRGULA
        const char PRIO_STMT = (char)200;               // STATEMENT

        readonly char[] OP_ABRE_PARENTESE = new char[] { PRIO_PARENTESE, (char)1, '(' };
        readonly char[] OP_NEGACAO = new char[] { PRIO_NEGACAO, (char)1, '!' };
        readonly char[] FUN_SIN = new char[] { PRIO_FUNC, (char)2, 'S', 'I' };
        readonly char[] FUN_COS = new char[] { PRIO_FUNC, (char)2, 'C', 'O' };
        readonly char[] FUN_TAN = new char[] { PRIO_FUNC, (char)2, 'T', 'G' };
        readonly char[] FUN_SUM = new char[] { PRIO_FUNC, (char)2, 'S', 'U' };
        readonly char[] OP_POTENCIA = new char[] { PRIO_POTENCIA, (char)1, '^' };
        readonly char[] OP_MULTIPLICACAO = new char[] { PRIO_MULTIPLICACAO, (char)1, '*' };
        readonly char[] OP_DIVISAO = new char[] { PRIO_DIVISAO, (char)1, '/' };
        readonly char[] OP_MODULO = new char[] { PRIO_MODULO, (char)1, '%' };
        readonly char[] OP_ADICAO = new char[] { PRIO_ADICAO, (char)1, '+' };
        readonly char[] OP_SUBTRACAO = new char[] { PRIO_SUBTRACAO, (char)1, '-' };
        readonly char[] OP_IGUAL_A = new char[] { PRIO_IGUAL_A, (char)1, '=' };
        readonly char[] OP_DIFERENTE_DE = new char[] { PRIO_DIFERENTE_DE, (char)2, '<', '>' };
        readonly char[] OP_MAIOR_QUE = new char[] { PRIO_MAIOR_QUE, (char)1, '>' };
        readonly char[] OP_MAIOR_OU_IGUAL_A = new char[] { PRIO_MAIOR_OU_IGUAL_A, (char)2, '>', '=' };
        readonly char[] OP_MENOR_QUE = new char[] { PRIO_MENOR_QUE, (char)1, '<' };
        readonly char[] OP_MENOR_OU_IGUAL_A = new char[] { PRIO_MENOR_OU_IGUAL_A, (char)2, '<', '=' };
        readonly char[] OP_AND = new char[] { PRIO_AND, (char)1, '&' };
        readonly char[] OP_ALL = new char[] { PRIO_ALL, (char)2, 'A', 'L' };
        readonly char[] OP_ANY = new char[] { PRIO_ANY, (char)2, 'A', 'N' };
        readonly char[] OP_BETWEEN = new char[] { PRIO_BETWEEN, (char)2, 'B', 'W' };
        readonly char[] OP_IN = new char[] { PRIO_IN, (char)2, 'I', 'N' };
        readonly char[] OP_LIKE = new char[] { PRIO_LIKE, (char)2, 'L', 'K' };
        readonly char[] OP_OR = new char[] { PRIO_OR, (char)1, '|' };
        readonly char[] OP_SOME = new char[] { PRIO_SOME, (char)2, 'S', 'M' };
        readonly char[] OP_VIRGULA = new char[] { PRIO_VIRGULA, (char)1, ',' };
        readonly char[] STMT_SELECT = new char[] { PRIO_STMT, (char)2, 'S', 'L' };
        readonly char[] CLSL_FROM = new char[] { PRIO_STMT, (char)2, 'F', 'R' };

        readonly char[] SCAN_MULT = new char[] { '*' };
        readonly char[] SCAN_DIV = new char[] { '/' };
        readonly char[] SCAN_SOMA = new char[] { '+' };
        readonly char[] SCAN_SUB = new char[] { '-' };

        public ExpressaoSql()
        {
            _capacidade_posfixa = -1;
        }

        int pos = 0;
        int inicio = -1;
        char[] op;

        int logicaPos = 0;

        bool sintaxeSelect = false;
        bool isFuncao = false;

        bool isNumero = false;
        bool isVariavel = false;
        bool isComando = false;
        bool isString = false;
        bool unario_ant = false;
        bool zero_esq = false;

        bool op_ant = false;
        bool op_ant_arit = false;
        bool op_ant_rel = false;
        bool op_ant_log = false;
        bool op_ant_vir = false;
        char[] ch_op_ant = new char[0];
        Stack<char[]> pilha_Op = new Stack<char[]>();
        Queue<char[]> fila_Str = new Queue<char[]>();
        StringBuilder sb = new StringBuilder();
        char[] posfixa = null;
        int posfixa_comp = 0;

        /// <summary>
        /// Converte a expressão infixa para Notação Polonesa Inversa
        /// </summary>
        /// <param name="infixa">Expressão em linguagem humana com parênteses, números, strings, variáveis, operadores aritméticos, relacionais e lógicos</param>
        /// <param name="comp">Comprimento da Array de retorno</param>
        /// <param name="out_strings">Saída da lista de Arrays de caracteres que representa as strings escritas entre aspas no meio da expressão</param>
        /// <returns>Retorna RPN</returns>
        public char[] InfixaParaPosfixa(char[] infixa, out Queue<char[]> out_strings)
        {
            if (_capacidade_posfixa == -1)
            {
                FastResize(ref posfixa, infixa.Length*2);
                posfixa_comp = posfixa.Length;
            }

            for (int i = 0; i < infixa.Length; i++)
            {
                if (!isVariavel)
                {
                    if (!isComando)
                    {
                        if (!isString)
                        {
                            if (infixa[i] == '\'')
                            {
                                op_ant = false;
                                isString = true;
                                continue;
                            }
                        }
                    }
                }

                isFuncao = false;
                op_ant_arit = false;
                op_ant_rel = false;
                op_ant_log = false;
                op_ant_vir = false;

                if (isComando)
                {
                    #region Comando
                    if (i == infixa.Length - 1 || !isLetra(infixa[i + 1]))
                    {
                        i++;
                        // Todos os comandos são considerados unários
                        // Declarações

                        if (i - inicio == 2)
                        {
                            #region Operador OR
                            if ((infixa[inicio] == 'o' || infixa[inicio] == 'O') &&
                                (infixa[inicio + 1] == 'r' || infixa[inicio + 1] == 'R'))
                            {
                                op_ant = true;
                                op_ant_log = true;
                                while (pilha_Op.Count > 0 &&
                                pilha_Op.Peek()[2] != '(' && PRIO_OR >= pilha_Op.Peek()[0])
                                {
                                    // O que está no topo de pilha tem mais prioridade
                                    op = pilha_Op.Pop();
                                    for (int b = 0; b < op[1]; b++)
                                        posfixa[pos++] = op[b + 2]; // Copia para posfixa
                                    posfixa[pos++] = ' ';
                                }

                                pilha_Op.Push(OP_OR);
                            }
                            #endregion
                            #region Operador IN
                            else if ((infixa[inicio] == 'i' || infixa[inicio] == 'I') &&
                                (infixa[inicio + 1] == 'n' || infixa[inicio + 1] == 'N'))
                            {
                                op_ant = true;
                                while (pilha_Op.Count > 0 &&
                                pilha_Op.Peek()[2] != '(' && PRIO_IN >= pilha_Op.Peek()[0])
                                {
                                    // O que está no topo de pilha tem mais prioridade
                                    op = pilha_Op.Pop();
                                    for (int b = 0; b < op[1]; b++)
                                        posfixa[pos++] = op[b + 2]; // Copia para posfixa
                                    posfixa[pos++] = ' ';
                                }

                                pilha_Op.Push(OP_IN);
                            }
                            #endregion
                        }
                        else if (i - inicio == 3)
                        {
                            #region Operador AND
                            if ((infixa[inicio] == 'a' || infixa[inicio] == 'A') &&
                                (infixa[inicio + 1] == 'n' || infixa[inicio + 1] == 'N') &&
                                (infixa[inicio + 2] == 'd' || infixa[inicio + 2] == 'D'))
                            {
                                op_ant_log = true;
                                op_ant = true;
                                while (pilha_Op.Count > 0 &&
                                pilha_Op.Peek()[2] != '(' && PRIO_AND >= pilha_Op.Peek()[0])
                                {
                                    // O que está no topo de pilha tem mais prioridade
                                    op = pilha_Op.Pop();
                                    for (int b = 0; b < op[1]; b++)
                                        posfixa[pos++] = op[b + 2]; // Copia para posfixa
                                    posfixa[pos++] = ' ';
                                }

                                pilha_Op.Push(OP_AND);
                            }
                            #endregion
                            #region Operador ALL
                            else if ((infixa[inicio] == 'a' || infixa[inicio] == 'A') &&
                                (infixa[inicio + 1] == 'l' || infixa[inicio + 1] == 'L') &&
                                (infixa[inicio + 2] == 'l' || infixa[inicio + 2] == 'L'))
                            {
                                op_ant = true;
                                while (pilha_Op.Count > 0 &&
                                pilha_Op.Peek()[2] != '(' && PRIO_ALL >= pilha_Op.Peek()[0])
                                {
                                    // O que está no topo de pilha tem mais prioridade
                                    op = pilha_Op.Pop();
                                    for (int b = 0; b < op[1]; b++)
                                        posfixa[pos++] = op[b + 2]; // Copia para posfixa
                                    posfixa[pos++] = ' ';
                                }

                                pilha_Op.Push(OP_ALL);
                            }
                            #endregion
                            #region Operador ANY
                            else if ((infixa[inicio] == 'a' || infixa[inicio] == 'A') &&
                                (infixa[inicio + 1] == 'n' || infixa[inicio + 1] == 'N') &&
                                (infixa[inicio + 2] == 'y' || infixa[inicio + 2] == 'Y'))
                            {
                                op_ant = true;
                                while (pilha_Op.Count > 0 &&
                                pilha_Op.Peek()[2] != '(' && PRIO_ANY >= pilha_Op.Peek()[0])
                                {
                                    // O que está no topo de pilha tem mais prioridade
                                    op = pilha_Op.Pop();
                                    for (int b = 0; b < op[1]; b++)
                                        posfixa[pos++] = op[b + 2]; // Copia para posfixa
                                    posfixa[pos++] = ' ';
                                }

                                pilha_Op.Push(OP_ANY);
                            }
                            #endregion
                            #region Operador NOT
                            else if ((infixa[inicio] == 'n' || infixa[inicio] == 'N') &&
                                (infixa[inicio + 1] == 'o' || infixa[inicio + 1] == 'O') &&
                                (infixa[inicio + 2] == 't' || infixa[inicio + 2] == 'T'))
                            {
                                op_ant_log = true;
                                op_ant = true;
                                while (pilha_Op.Count > 0 &&
                                pilha_Op.Peek()[2] != '(' && PRIO_NEGACAO >= pilha_Op.Peek()[0])
                                {
                                    // O que está no topo de pilha tem mais prioridade
                                    op = pilha_Op.Pop();
                                    for (int b = 0; b < op[1]; b++)
                                        posfixa[pos++] = op[b + 2]; // Copia para posfixa
                                    posfixa[pos++] = ' ';
                                }

                                pilha_Op.Push(OP_NEGACAO);
                            }
                            #endregion
                            else if (infixa[inicio] == 'c' || infixa[inicio] == 'C')
                            {
                                #region COS
                                if ((infixa[inicio + 1] == 'o' || infixa[inicio + 1] == 'O') &&
                                  (infixa[inicio + 2] == 's' || infixa[inicio + 2] == 'S'))
                                {
                                    op_ant = true;
                                    isFuncao = true;
                                    while (pilha_Op.Count > 0 &&
                                    pilha_Op.Peek()[2] != '(' && PRIO_FUNC >= pilha_Op.Peek()[0])
                                    {
                                        // O que está no topo de pilha tem mais prioridade
                                        op = pilha_Op.Pop();
                                        for (int b = 0; b < op[1]; b++)
                                            posfixa[pos++] = op[b + 2]; // Copia para posfixa
                                        posfixa[pos++] = ' ';
                                    }

                                    pilha_Op.Push(FUN_COS);
                                }
                                #endregion
                            }
                            else if (infixa[inicio] == 's' || infixa[inicio] == 'S')
                            {
                                #region SIN
                                if ((infixa[inicio + 1] == 'i' || infixa[inicio + 1] == 'I') &&
                                  (infixa[inicio + 2] == 'n' || infixa[inicio + 2] == 'N'))
                                {
                                    op_ant = true;
                                    isFuncao = true;
                                    while (pilha_Op.Count > 0 &&
                                    pilha_Op.Peek()[2] != '(' && PRIO_FUNC >= pilha_Op.Peek()[0])
                                    {
                                        // O que está no topo de pilha tem mais prioridade
                                        op = pilha_Op.Pop();
                                        for (int b = 0; b < op[1]; b++)
                                            posfixa[pos++] = op[b + 2]; // Copia para posfixa
                                        posfixa[pos++] = ' ';
                                    }

                                    pilha_Op.Push(FUN_SIN);
                                }
                                #endregion

                                #region SUM
                                else if ((infixa[inicio + 1] == 'u' || infixa[inicio + 1] == 'U') &&
                                  (infixa[inicio + 2] == 'm' || infixa[inicio + 2] == 'M'))
                                {
                                    op_ant = true;
                                    isFuncao = true;
                                    while (pilha_Op.Count > 0 &&
                                    pilha_Op.Peek()[2] != '(' && PRIO_FUNC >= pilha_Op.Peek()[0])
                                    {
                                        // O que está no topo de pilha tem mais prioridade
                                        op = pilha_Op.Pop();
                                        for (int b = 0; b < op[1]; b++)
                                            posfixa[pos++] = op[b + 2]; // Copia para posfixa
                                        posfixa[pos++] = ' ';
                                    }

                                    pilha_Op.Push(FUN_SUM);
                                }
                                #endregion
                            }
                            else if (infixa[inicio] == 't' || infixa[inicio] == 'T')
                            {
                                #region TAN
                                if ((infixa[inicio + 1] == 'a' || infixa[inicio + 1] == 'A') &&
                                  (infixa[inicio + 2] == 'n' || infixa[inicio + 2] == 'N'))
                                {
                                    op_ant = true;
                                    isFuncao = true;
                                    while (pilha_Op.Count > 0 &&
                                    pilha_Op.Peek()[2] != '(' && PRIO_FUNC >= pilha_Op.Peek()[0])
                                    {
                                        // O que está no topo de pilha tem mais prioridade
                                        op = pilha_Op.Pop();
                                        for (int b = 0; b < op[1]; b++)
                                            posfixa[pos++] = op[b + 2]; // Copia para posfixa
                                        posfixa[pos++] = ' ';
                                    }

                                    pilha_Op.Push(FUN_TAN);
                                }
                                #endregion
                            }
                        }
                        else if (i - inicio == 4)
                        {
                            #region Cláusula FROM
                            if ((infixa[inicio] == 'f' || infixa[inicio] == 'F') &&
                                (infixa[inicio + 1] == 'r' || infixa[inicio + 1] == 'R') &&
                                (infixa[inicio + 2] == 'o' || infixa[inicio + 2] == 'O') &&
                                (infixa[inicio + 3] == 'm' || infixa[inicio + 3] == 'M'))
                            {
                                logicaPos = 3;

                                op_ant = true;
                                while (pilha_Op.Count > 0 &&
                                    pilha_Op.Peek()[2] != '(' && PRIO_STMT > pilha_Op.Peek()[0]) // Operador Unário
                                {
                                    // O que está no topo da pilha tem mais prioridade
                                    op = pilha_Op.Pop();
                                    for (int b = 0; b < op[1]; b++)
                                        posfixa[pos++] = op[b + 2]; // Copia para posfixa
                                    posfixa[pos++] = ' ';
                                }

                                pilha_Op.Push(CLSL_FROM);
                            }
                            #endregion
                        }
                        else if (i - inicio == 6)
                        {
                            #region Declaração SELECT
                            if ((infixa[inicio] == 's' || infixa[inicio] == 'S') &&
                                (infixa[inicio + 1] == 'e' || infixa[inicio + 1] == 'E') &&
                                (infixa[inicio + 2] == 'l' || infixa[inicio + 2] == 'L') &&
                                (infixa[inicio + 3] == 'e' || infixa[inicio + 3] == 'E') &&
                                (infixa[inicio + 4] == 'c' || infixa[inicio + 4] == 'C') &&
                                (infixa[inicio + 5] == 't' || infixa[inicio + 5] == 'T'))
                            {
                                sintaxeSelect = true;
                                logicaPos = 1;
                                op_ant = true;
                                while (pilha_Op.Count > 0 &&
                                    pilha_Op.Peek()[2] != '(' && PRIO_STMT > pilha_Op.Peek()[0]) // Operador Unário
                                {
                                    // O que está no topo da pilha tem mais prioridade
                                    op = pilha_Op.Pop();
                                    for (int b = 0; b < op[1]; b++)
                                        posfixa[pos++] = op[b + 2]; // Copia para posfixa
                                    posfixa[pos++] = ' ';
                                }

                                pilha_Op.Push(STMT_SELECT);
                            }
                            #endregion
                        }
                        if (i == infixa.Length) i--;
                        isComando = false;
                    }
                    else if (isLetra(infixa[i])) continue;
                    else
                    {
                        throw new Exception($"'{infixa[i]}' não esperado");
                    }

                    #endregion
                }
                else if (isVariavel)
                {
                    if (infixa[i] == ' ' || infixa[i] == ',' || infixa[i] == ')')
                    {
                        isVariavel = false;
                    }
                    else if (isCaractereVar(infixa[i]))
                    {
                        posfixa[pos++] = infixa[i];
                    }
                    else
                    {
                        throw new Exception("Uso incorreto de '@'. Nome de variável inválido");
                    }
                    //continue;
                }
                else if (isString) // Converte string em variável
                {
                    #region String
                    if (infixa[i] == '\\' && infixa[i + 1] == '\'')
                    {
                        sb.Append("\'"); i++;
                        continue;
                    }
                    else if (infixa[i] == '\'') // '
                    {
                        isString = false;
                        fila_Str.Enqueue(sb.ToString().ToCharArray());           // Aloca a string na memória

                        // Cria o identificador da string alocada
                        posfixa[pos++] = '$'; // Símbolo String
                        posfixa[pos++] = (char)((fila_Str.Count - 1) + ((int)'0')); // Índice da string

                        sb.Clear();
                        continue;
                    }

                    sb.Append(infixa[i]);
                    continue;
                    #endregion
                }
                else if (isLetra(infixa[i]))
                {
                    if (i == 0 || (i > 0 && (infixa[i - 1] == ' ' || infixa[i - 1] == '(' ||
                        op_ant_arit || op_ant_rel /*Permite juntar operadores aritméticos e relacionais com o comando*/)))
                    {
                        if (!isComando)
                        {
                            isComando = true;
                            inicio = i;
                            continue;
                        }
                    }
                }
                else if (infixa[i] == '@')
                {
                    #region Variável
                    op_ant = false;
                    if (i > 0 && infixa[i - 1] != ' ') // Ex.: 12@
                    {
                        throw new Exception("Uso incorreto de '@'. Insira espaço antes de '@'");
                    }
                    else if (i == infixa.Length - 1 || infixa[i + 1] == ' ') // Ex.: @
                    {
                        throw new Exception("Uso incorreto de '@'. Insira o nome da variável");
                    }

                    isVariavel = true;
                    posfixa[pos++] = '@';
                    continue;
                    #endregion
                }
                else if (isDigito(infixa[i])) // Checa se é número
                {
                    #region Escaneamento do número
                    op_ant = false;
                    if (!isNumero) // Checa se o primeiro caractere da esquerda é um número
                    {
                        isNumero = true;
                        if (infixa[i] == '0') zero_esq = true;
                        if (pos > 0 && !unario_ant && i > 0 && posfixa[pos - 1] != ' ')
                        {
                            posfixa[pos++] = ' ';
                            unario_ant = false;
                        }
                    }

                    if (zero_esq && infixa[i] == '0')
                    {
                        if (i == infixa.Length - 1) // Adiciona Zero no último índice
                        {
                            posfixa[pos++] = '0';
                            zero_esq = false;
                        }
                        continue; // Ignora zeros a esquerda
                    }
                    zero_esq = false;
                    posfixa[pos++] = infixa[i];
                    continue; // Pula para o próximo número ou espaço em branco
                    #endregion
                }
                else
                {
                    #region Scanneamento final do número
                    if (isNumero)
                    {
                        if (infixa[i] == '.')
                        {
                            posfixa[pos++] = '.';
                            continue;
                        }
                        if (zero_esq) // Insere o valor Zero
                        {
                            posfixa[pos++] = '0';
                            zero_esq = false;
                        }
                        posfixa[pos++] = ' ';
                        isNumero = false;
                    }
                    #endregion
                }

                #region Operador (
                if (infixa[i] == '(')
                {
                    op_ant = true;
                    pilha_Op.Push(OP_ABRE_PARENTESE); // Adiciona na pilha
                    if (i > 0 && pos > 0 && posfixa[pos - 1] != ' ') posfixa[pos++] = ' ';
                }
                #endregion
                #region Operador )
                else if (infixa[i] == ')')
                {
                    sintaxeSelect = false;
                    logicaPos = 0;

                    while (pilha_Op.Count > 0 &&         // Enquanto conter elementos
                           pilha_Op.Peek()[2] != '(')    // Desempilha e lança todos no final da posfixa até encontrar um '('
                    {
                        op = pilha_Op.Pop();
                        for (int b = 0; b < op[1]; b++)
                            posfixa[pos++] = op[b + 2]; // Copia para posfixa
                        posfixa[pos++] = ' ';
                    }

                    if (pilha_Op.Count == 0)
                    {
                        throw new Exception("Expressão inválida. Abre-parêntese está faltando");
                    }

                    if (pilha_Op.Peek()[2] == '(')
                        pilha_Op.Pop();
                    else
                        throw new Exception("Expressão inválida. Fecha-parêntese está faltando");
                }
                #endregion
                else if (infixa[i] == ' ')
                {
                    #region Separador de Argumentos
                    if (pos > 0)
                    {
                        if (posfixa[pos - 1] != ' ') posfixa[pos++] = ' ';
                    }
                    #endregion
                }
                else if (Operador(new char[] { '*' }, false, ref infixa, i, PRIO_MULTIPLICACAO, OP_MULTIPLICACAO)) { }
                else if (Operador(new char[] { '/' }, false, ref infixa, i, PRIO_DIVISAO, OP_DIVISAO)) { }
                else if (Operador(new char[] { '%' }, false, ref infixa, i, PRIO_MODULO, OP_MODULO)) { }

                else if (infixa[i] == '+')
                {
                    #region Operador unário +
                    if (op_ant || pos == 0 &&            // Sucede um operador anterior ou posição inicial
                        (isDigito(infixa[i + 1]) ||      // Operador precede valor numérico
                        isLetra(infixa[i + 1])))         // Operador precede variável
                    {
                        if (unario_ant) // Já existe um operador unário anterior?
                        {
                            posfixa[pos++] = '+'; // Insere o operador unário + após o operador unário anterior
                        }
                        else
                            // Desconsidera o operador unário + se não houver nenhum operador unário anterior
                            unario_ant = true;
                    }
                    #endregion
                    #region Operador +
                    else
                    {
                        ch_op_ant = new char[1];
                        ch_op_ant[0] = '+';

                        op_ant = true;
                        op_ant_arit = true;
                        while (pilha_Op.Count > 0 &&
                        pilha_Op.Peek()[2] != '(' && PRIO_ADICAO >= pilha_Op.Peek()[0])
                        {
                            // O que está no topo de pilha tem mais prioridade
                            op = pilha_Op.Pop();
                            for (int b = 0; b < op[1]; b++)
                                posfixa[pos++] = op[b + 2]; // Copia para posfixa
                            posfixa[pos++] = ' ';
                        }

                        pilha_Op.Push(OP_ADICAO);
                    }
                    #endregion
                }
                else if (infixa[i] == '-')
                {
                    #region Operador Unário -
                    if (op_ant || pos == 0 &&           // Sucede um operador anterior ou posição inicial
                        (isDigito(infixa[i + 1]) ||     // Operador unário precede valor numérico
                        isLetra(infixa[i + 1])))        // Operador unário precede variável
                    {
                        unario_ant = true;
                        posfixa[pos++] = '-'; // Insere o operador unário - no final da posfixa
                    }
                    #endregion
                    #region Operador -
                    else
                    {
                        ch_op_ant = new char[1];
                        ch_op_ant[0] = '-';

                        op_ant = true;
                        op_ant_arit = true;
                        while (pilha_Op.Count > 0 &&
                            pilha_Op.Peek()[2] != '(' && PRIO_SUBTRACAO >= pilha_Op.Peek()[0])
                        {
                            // O que está no topo de pilha tem mais prioridade
                            op = pilha_Op.Pop();
                            for (int b = 0; b < op[1]; b++)
                                posfixa[pos++] = op[b + 2]; // Copia para posfixa
                            posfixa[pos++] = ' ';
                        }
                        pilha_Op.Push(OP_SUBTRACAO);
                    }
                    #endregion
                }
                else if (Operador(new char[] { '^' }, false, ref infixa, i, PRIO_POTENCIA, OP_POTENCIA)) { }
                else if (Operador(new char[] { '=', '=' }, false, ref infixa, i, PRIO_IGUAL_A, OP_IGUAL_A)) { }
                else if (Operador(new char[] { '>', '=' }, false, ref infixa, i, PRIO_MAIOR_OU_IGUAL_A, OP_MAIOR_OU_IGUAL_A)) { }
                else if (Operador(new char[] { '>' }, false, ref infixa, i, PRIO_MAIOR_QUE, OP_MAIOR_QUE)) { }
                else if (infixa[i] == '!') // NOT
                {
                    #region Operador !=
                    if ((i + 1 < infixa.Length) && infixa[i + 1] == '=') // !=
                    {
                        ch_op_ant = new char[2];
                        ch_op_ant[0] = '!';
                        ch_op_ant[1] = '=';

                        op_ant_rel = true;
                        op_ant = true;
                        while (pilha_Op.Count > 0 &&
                               pilha_Op.Peek()[2] != '(' && PRIO_DIFERENTE_DE >= pilha_Op.Peek()[0])
                        {
                            // O que está no topo de pilha tem mais prioridade
                            op = pilha_Op.Pop();
                            for (int b = 0; b < op[1]; b++)
                                posfixa[pos++] = op[b + 2]; // Copia para posfixa
                            posfixa[pos++] = ' ';
                        }

                        pilha_Op.Push(OP_DIFERENTE_DE);
                        i++;
                    }
                    #endregion
                    #region Operador unário !
                    else // !
                    {
                        op_ant_rel = true;
                        op_ant = true;
                        while (pilha_Op.Count > 0 &&
                        pilha_Op.Peek()[2] != '(' && PRIO_NEGACAO > pilha_Op.Peek()[0])
                        {
                            // O que está no topo de pilha tem mais prioridade
                            op = pilha_Op.Pop();
                            for (int b = 0; b < op[1]; b++)
                                posfixa[pos++] = op[b + 2]; // Copia para posfixa
                            posfixa[pos++] = ' ';
                        }

                        pilha_Op.Push(OP_NEGACAO);
                    }
                    #endregion
                }
                
                else if (Operador(new char[] { '!', '=' }, false, ref infixa, i, PRIO_DIFERENTE_DE, OP_DIFERENTE_DE)) { }
                else if (Operador(new char[] { '<', '>' }, false, ref infixa, i, PRIO_DIFERENTE_DE, OP_DIFERENTE_DE)) { }
                else if (Operador(new char[] { '<', '=' }, false, ref infixa, i, PRIO_MENOR_OU_IGUAL_A, OP_MENOR_OU_IGUAL_A)) { }
                else if (Operador(new char[] { '<' }, false, ref infixa, i, PRIO_MENOR_QUE, OP_MENOR_QUE)) { }
                else if (Operador(new char[] { '&', '&' }, false, ref infixa, i, PRIO_AND, OP_AND)) { }
                else if (Operador(new char[] { '|', '|' }, false, ref infixa, i, PRIO_OR, OP_OR)) { }
                else if (OperadorUnario(new char[] { '!' }, false, ref infixa, i, PRIO_NEGACAO, OP_NEGACAO)) { }

                #region Operador ,
                else if (infixa[i] == ',')
                {
                    op_ant = true;
                    op_ant_vir = true;
                    while (pilha_Op.Count > 0 &&
                    pilha_Op.Peek()[2] != '(' && PRIO_VIRGULA > pilha_Op.Peek()[0])
                    {
                        // O que está no topo de pilha tem mais prioridade
                        op = pilha_Op.Pop();
                        for (int b = 0; b < op[1]; b++)
                            posfixa[pos++] = op[b + 2]; // Copia para posfixa
                        posfixa[pos++] = ' ';
                    }

                    pilha_Op.Push(OP_VIRGULA);
                }
                #endregion
                else if (infixa[i] == ';') break;
                //else
                //{
                //    if (isVariavel) continue;
                //    throw new Exception("'" + new string(infixa, i, 1) + "' não esperado");
                //}
            }

            while (pilha_Op.Count > 0)
            {
                if (pos > 0 && posfixa[pos - 1] != ' ') posfixa[pos++] = ' ';
                if (pilha_Op.Peek()[2] == '(')
                {
                    throw new Exception("Expressão inválida. Fecha-parêntese está faltando");
                }
                op = pilha_Op.Pop();
                for (int b = 0; b < op[1]; b++) posfixa[pos++] = op[b + 2];
            }

            if (op_ant_arit)
            {
                throw new Exception($"Erro de expressão aritmética. É esperado um valor do lado direito do operador {new string(ch_op_ant)}");
            }
            else if (op_ant_log)
            {
                throw new Exception($"Erro de expressão lógica. É esperado um valor do lado direito do operador {new string(ch_op_ant)}");
            }
            else if (op_ant_rel)
            {
                throw new Exception($"Erro de expressão relacional. É esperado um valor do lado direito do operador {new string(ch_op_ant)}");
            }
            else  if (sintaxeSelect && logicaPos == 1)
            {
                if (op_ant_vir)
                {
                    throw new Exception("Erro de sintaxe. Era esperado uma expressão ou nome da coluna após a vírgula");
                }

                //throw new Exception("Erro de sintaxe. É esperado uma expressão ou nome de coluna");
            }

            _compRPN = pos;
            out_strings = fila_Str;
            return posfixa;
        }

        private bool Operador(char[] op, bool espaco_em_branco_apos, ref char[] infixa, int i, char precedencia, char[] empilha_op)
        {
            if (i + op.Length - 1 > infixa.Length - 1) // Maior que o comprimento infixa
            {
                return false;
            }

            // Checa a existência do operador na infixa na posição i
            for (int b = 0; b < op.Length; b++)
            {
                if (i + b > infixa.Length - 1 && infixa[i + b] != op[b]) return false;
                else if (b == op.Length - 1)
                {
                    if (i + b == infixa.Length - 1)
                    {
                        throw new Exception($"Era esperado um valor do lado direito do operador '{new string(op)}'");
                    }
                    else if (infixa[i + b] != ' ') return false;    // Caractere após o operador não é um espaço em branco
                }
            }

            ch_op_ant = op;     // Salva o último operador escaneado
            op_ant_rel = true;  // Tipo de operador
            op_ant = true;      // Operador anterior escaneado

            while (pilha_Op.Count > 0 &&
                pilha_Op.Peek()[2] != '(' && precedencia >= pilha_Op.Peek()[0])
            {
                op = pilha_Op.Pop();
                for (int b = 0; b < op[1]; b++) posfixa[pos++] = op[b + 2];
                posfixa[pos++] = ' ';
            }

            pilha_Op.Push(empilha_op);
            i++;

            return true;
        }

        private bool OperadorUnario(char[] op, bool espaco_em_branco_apos, ref char[] infixa, int i, char precedencia, char[] empilha_op)
        {
            // Checa a existência do operador na infixa na posição i
            for (int b = 0; b < op.Length; b++)
            {
                if (infixa[i + b] != op[b]) return false;
                else if (b == op.Length - 1 && espaco_em_branco_apos)
                {
                    if (i + b < infixa.Length - 1) return true;     // Limite de comprimento do infixa
                    else if (infixa[b + 1] != ' ') return false;    // Caractere após o operador não é um espaço em branco
                }
            }

            ch_op_ant = op;     // Salva o último operador escaneado
            op_ant_rel = true;  // Tipo de operador
            op_ant = true;      // Operador anterior escaneado

            while (pilha_Op.Count > 0 &&
                pilha_Op.Peek()[2] != '(' && precedencia > pilha_Op.Peek()[0])
            {
                op = pilha_Op.Pop();
                for (int b = 0; b < op[1]; b++) posfixa[pos++] = op[b + 2];
                posfixa[pos++] = ' ';
            }

            pilha_Op.Push(empilha_op);
            i++;

            return true;
        }

        /// <summary>
        /// Calculadora de Notação Polonesa Inversa
        /// </summary>
        /// <param name="rpn"></param>
        /// <param name="in_strings"></param>
        /// <param name="in_variaveis"></param>
        /// <param name="out_variaveis"></param>
        /// <returns></returns>
        //public unsafe object CalcularRPN(char* rpn, Queue<char[]> in_strings,
        //variavel[] in_variaveis, out variavel[] out_variaveis)
        public unsafe object CalcularRPN(char[] rpn, Queue<char[]> in_strings,
            variavel[] in_variaveis, out variavel[] out_variaveis)
        {
            out_variaveis = null;

            byte tipo = 0;

            bool select = false;

            int parametros = 1;
            bool isVariavel = false;
            bool isString = false;
            bool isNumero = false;
            bool isNegativo = false;
            int inicio = -1;
            int elemento = -1;
            char[] strArr;
            char[] nomeVar;
            bool boolExp = false;
            int colunas = 0;

            //fixed (double* pilha = pilhaArray)
            {
                int pilhaPos = 0;

                for (int i = 0; i < _compRPN; i++)
                {
                    if (isVariavel)
                    {
                        if (isCaractereVar(rpn[i]))
                        {
                            continue;
                        }
                        else if (rpn[i] == ' ' || i == _compRPN - 1)
                        {
                            nomeVar = new char[i - inicio - 1];
                            for (int b = inicio + 1, c = 0; b < i; b++, c++)
                            {
                                nomeVar[c] = rpn[b];
                            }
                            isVariavel = false;
                        }
                        else
                        {
                            throw new Exception($"'{rpn[i]}' não esperado");
                        }
                    }
                    else if (isString)
                    {
                        if (isDigito(rpn[i]))
                        {
                            continue;
                        }
                        else if (rpn[i] == ' ' || i == _compRPN - 1)
                        {
                            #region Copia a string para dblArr
                            elemento = (int)CustomToDecimal(rpn, inicio + 1, i - 1, false);

                            if (isNumerico(strArr = in_strings.ElementAt(elemento), out isNegativo))
                            {
                                // CAST: Converte String '' em Numérico
                                unsafe
                                {
                                    fixed (char* pStr = strArr)
                                    {
                                        _dblArr[pilhaPos] = (double)CustomToDecimal(strArr, isNegativo ? 1 : 0, strArr.Length - 1, isNegativo);
                                    }
                                }
                            }
                            else
                            {
                                // String 
                                unsafe
                                {
                                    fixed (char* pStr = _strArr)
                                    {
                                        for (int b = 0; b < strArr.Length; b++)
                                        {
                                            // Copia a string sem as aspas
                                            pStr[pilhaPos * CHAR_SIZE + b] = strArr[b];
                                        }
                                    }
                                }
                            }

                            #endregion

                            pilhaPos++;
                            isString = false;
                            isNegativo = false;
                            continue;
                        }
                        else
                        {
                            throw new Exception($"'{rpn[i]}' não esperado");
                        }
                    }
                    else if (isDigito(rpn[i]))
                    {
                        if (!isNumero)
                        {
                            isNumero = true;
                            inicio = i;
                        }

                        continue;
                    }
                    else
                    {
                        if (isNumero)
                        {
                            if (rpn[i] == '.') continue;

                            _dblArr[pilhaPos++] = (double)CustomToDecimal(rpn, inicio, i - 1, isNegativo);
                            isNumero = false;
                            isNegativo = false;
                        }
                    }

                    #region Variáveis e Strings
                    if (rpn[i] == ' ')
                    {
                        continue;
                    }
                    else if (rpn[i] == '@')
                    {
                        if (!isVariavel)
                        {
                            isVariavel = true;
                            inicio = i;
                            continue;
                        }
                    }
                    else if (rpn[i] == '$')
                    {
                        if (!isString)
                        {
                            isString = true;
                            inicio = i;
                            continue;
                        }
                    }
                    else if (rpn[i] == ',')
                    {
                        parametros++;
                        continue;
                    }
                    #endregion
                    #region Operadores Aritméticos
                    else if (rpn[i] == '*')
                    {
                        _dblArr[pilhaPos - 2] = _dblArr[pilhaPos - 2] * _dblArr[pilhaPos - 1];
                        pilhaPos -= 1;
                    }
                    else if (rpn[i] == '/')
                    {
                        _dblArr[pilhaPos - 2] = _dblArr[pilhaPos - 2] / _dblArr[pilhaPos - 1];
                        pilhaPos -= 1;
                    }
                    else if (rpn[i] == '-')
                    {
                        if (isDigito(rpn[i + 1]))
                            isNegativo = true;  // Número negativo
                        else
                        {
                            _dblArr[pilhaPos - 2] = _dblArr[pilhaPos - 2] - _dblArr[pilhaPos - 1];  // Subtração
                            pilhaPos -= 1;
                        }
                    }
                    else if (rpn[i] == '+')
                    {
                        if (_strArr[pilhaPos - 2, 0] > 0) // Concatenação de strings
                        {

                        }
                        else
                        {
                            _dblArr[pilhaPos - 2] = _dblArr[pilhaPos - 2] + _dblArr[pilhaPos - 1];
                        }
                        pilhaPos -= 1;
                    }
                    else if (rpn[i] == '^')
                    {
                        _dblArr[pilhaPos - 2] = Math.Pow(_dblArr[pilhaPos - 2], _dblArr[pilhaPos - 1]);
                        pilhaPos -= 1;
                    }
                    else if (rpn[i] == '%')
                    {
                        _dblArr[pilhaPos - 2] = _dblArr[pilhaPos - 2] % _dblArr[pilhaPos - 1];
                        pilhaPos -= 1;
                    }
                    #endregion
                    #region Operadores Relacionais
                    else if (rpn[i] == '=') // '=
                    {
                        boolExp = double.Equals(_dblArr[pilhaPos - 2], _dblArr[pilhaPos - 1]);
                        _dblArr[pilhaPos - 2] = *((byte*)(&boolExp));
                        pilhaPos -= 1;
                    }
                    else if (rpn[i] == '<')
                    {
                        if (rpn[i + 1] == '>') // <>
                        {
                            boolExp = !double.Equals(_dblArr[pilhaPos - 2], _dblArr[pilhaPos - 1]);
                            i++;
                        }
                        else if (rpn[i + 1] == '=') // <=
                        {
                            boolExp = _dblArr[pilhaPos - 2] <= _dblArr[pilhaPos - 1];
                            i++;
                        }
                        else // <
                        {
                            boolExp = _dblArr[pilhaPos - 2] < _dblArr[pilhaPos - 1];
                        }

                        _dblArr[pilhaPos - 2] = *((byte*)(&boolExp));
                        pilhaPos -= 1;
                    }
                    else if (rpn[i] == '>')
                    {
                        if (rpn[i + 1] == '=') // >=
                        {
                            boolExp = _dblArr[pilhaPos - 2] >= _dblArr[pilhaPos - 1];
                            i++;
                        }
                        else // >
                        {
                            boolExp = _dblArr[pilhaPos - 2] > _dblArr[pilhaPos - 1];
                        }

                        _dblArr[pilhaPos - 2] = *((byte*)(&boolExp));
                        pilhaPos -= 1;
                    }
                    #endregion
                    #region Operadores Lógicos
                    else if (rpn[i] == '&')
                    {
                        if (_dblArr[pilhaPos - 2] == 1 && _dblArr[pilhaPos - 1] == 1)       // 1 AND 1 = 1
                            _dblArr[pilhaPos - 2] = 1;
                        else if (_dblArr[pilhaPos - 2] == 1 && _dblArr[pilhaPos - 1] == 0)  // 1 AND 0 = 0
                            _dblArr[pilhaPos - 2] = 0;
                        else if (_dblArr[pilhaPos - 2] == 0 && _dblArr[pilhaPos - 1] == 1)  // 0 AND 1 = 0
                            _dblArr[pilhaPos - 2] = 0;
                        else if (_dblArr[pilhaPos - 2] == 0 && _dblArr[pilhaPos - 1] == 0)  // 0 AND 0 = 0
                            _dblArr[pilhaPos - 2] = 0;
                        else
                        {
                            throw new Exception("Expressão inválida. Não é possível utilizar o operador AND em valores que resultarem ser diferentes de 0 e 1");
                        }
                        pilhaPos -= 1;
                    }
                    else if (rpn[i] == '|')
                    {
                        if (_dblArr[pilhaPos - 2] == 0 && _dblArr[pilhaPos - 1] == 0)       // 0 OR 0 = 0
                            _dblArr[pilhaPos - 2] = 0;
                        else if (_dblArr[pilhaPos - 2] == 0 && _dblArr[pilhaPos - 1] == 1)  // 0 OR 1 = 1
                            _dblArr[pilhaPos - 2] = 1;
                        else if (_dblArr[pilhaPos - 2] == 1 && _dblArr[pilhaPos - 1] == 0)  // 1 OR 0 = 1
                            _dblArr[pilhaPos - 2] = 1;
                        else if (_dblArr[pilhaPos - 2] == 1 && _dblArr[pilhaPos - 1] == 1)  // 1 OR 1 = 1
                            _dblArr[pilhaPos - 2] = 1;
                        else
                        {
                            throw new Exception("Expressão inválida. Não é possível utilizar o operador OR em valores que resultarem ser diferentes de 0 e 1");
                        }
                        pilhaPos -= 1;
                    }
                    else if (rpn[i] == '!')
                    {
                        if (_dblArr[pilhaPos - 1] == 1)         // !1 = 0
                            _dblArr[pilhaPos - 1] = 0;
                        else if (_dblArr[pilhaPos - 1] == 0)    // !0 = 1
                            _dblArr[pilhaPos - 1] = 1;
                        else
                        {
                            throw new Exception("Expressão inválida. Não é possível negar valores que resultarem ser diferentes de 0 e 1");
                        }
                    }
                    #endregion
                    #region Funções e Comandos
                    else if (rpn[i] == 'c' || rpn[i] == 'C')
                    {
                        #region CO - COS
                        if (rpn[i + 1] == 'o' || rpn[i + 1] == 'O') // CO (COS)
                        {
                            if (rpn[i + 2] == ' ' || i + 2 == _compRPN)
                            {
                                if (parametros > 1)
                                {
                                    throw new Exception("A função COS só permite 1 parâmetro");
                                }

                                // Executa a função COS (COSSENO)
                                _dblArr[pilhaPos - 1] = Math.Cos(_dblArr[pilhaPos - 1]);
                                i += 2;

                                parametros = 1;
                            }
                        }
                        #endregion
                    }
                    else if (rpn[i] == 'f' || rpn[i] == 'F')
                    {
                        #region FR - FROM
                        if (rpn[i + 1] == 'r' || rpn[i + 1] == 'R')
                        {
                            if (rpn[i + 2] == ' ' || i + 2 == _compRPN)
                            {
                                colunas = parametros - 1;

                                if (colunas == 0)
                                {
                                    throw new Exception("A instrução SELECT deve possuir pelo menos uma expressão ou um nome de coluna");
                                }

                                i += 2;
                            }
                        }
                        #endregion
                    }
                    else if (rpn[i] == 's' || rpn[i] == 'S')
                    {
                        #region SL - SELECT
                        if (rpn[i + 1] == 'l' || rpn[i + 1] == 'L') // SL (SELECT)
                        {
                            

                            if (rpn[i + 2] == ' ' || i + 2 == _compRPN)
                            {
                                i += 2; // Pula o SL (SELECT)
                            }
                            else if (i == _compRPN - 2)
                            {
                                

                                select = true;
                                break;
                            }
                        }
                        #endregion
                        #region SU - SUM
                        else if (rpn[i + 1] == 'u' || rpn[i + 1] == 'U') // SU (SUM)
                        {
                            if (rpn[i + 2] == ' ' || i + 2 == _compRPN)
                            {
                                if (parametros > 2)
                                {
                                    throw new Exception("A função SUM só permite 2 parâmetros");
                                }

                                // Executa a função SUM (SOMA)
                                i += 2;
                                parametros = 1;
                            }
                        }
                        #endregion
                        #region SI - SIN
                        else if (rpn[i + 1] == 'i' || rpn[i + 1] == 'I') // SI (SIN)
                        {
                            if (rpn[i + 2] == ' ' || i + 2 == _compRPN)
                            {
                                if (parametros > 1)
                                {
                                    throw new Exception("A função SIN só permite 1 parâmetro");
                                }

                                // Executa a função SIN (SENO)
                                _dblArr[pilhaPos - 1] = Math.Sin(_dblArr[pilhaPos - 1]);
                                i += 2;
                                parametros = 1;
                            }
                        }
                        #endregion
                    }
                    else if (rpn[i] == 't' || rpn[i] == 'T')
                    {
                        #region TG - TAN
                        if (rpn[i + 1] == 'g' || rpn[i + 1] == 'G') // TG (TAN)
                        {
                            if (rpn[i + 2] == ' ' || i + 2 == _compRPN)
                            {
                                if (parametros > 1)
                                {
                                    throw new Exception("A função TAN só permite 1 parâmetro");
                                }

                                // Executa a função TAN (TANGENTE)
                                _dblArr[pilhaPos - 1] = Math.Tan(_dblArr[pilhaPos - 1]);
                                i += 2;
                                parametros = 1;
                            }
                        }
                        #endregion
                    }
                    #endregion
                    else
                    {
                        throw new Exception($"'{rpn[i]}' não esperado");
                    }
                }

                if (tipo == 1) // String
                {
                    return ' ';
                }
                else if (tipo == 2) // Numerico
                {
                    return _dblArr[0];
                }
                else
                    return _dblArr[0];
                //return null;
            }
        }

        double Sum(params double[] p)
        {
            double ret = 0D;
            for (int i = 0; i < p.Length; i++)
            {
                ret += p[i];
            }
            return ret;
        }

        public static unsafe decimal CustomToDecimal(char[] chArray, int offset, int max, bool negative = false)
        {
            //public static unsafe decimal CustomToDecimal(char* chArray, int offset, int max, bool negative = false)
            long n = 0;
            int count = (max - offset) + 1;
            int decimalPosition = count;
            bool dot = false;
            for (int pos = offset; pos <= max; pos++)
            {
                if (chArray[pos] == '.')
                {
                    dot = true;
                    decimalPosition = (pos - offset);
                }
                else
                    n = (n * 10) + (int)(chArray[pos] - '0');
            }

            return new decimal((int)n, (int)(n >> 32), 0, negative, (byte)((count - decimalPosition) - (dot ? 1 : 0)));
        }

        public static bool isDigito(char ch) => ch >= '0' && ch <= '9';

        //public static bool isString(char[] valor)
        //{
        //    return (valor[0] == '\'') && valor[valor.Length - 1] == '\'';
        //}

        public static bool isLetra(char ch)
        {
            if (ch >= 'a' && ch <= 'z')
                return true;
            else if(ch >= 'A' && ch <= 'Z')
                return true;
            else
                return false;
        }

        public static bool isCaractereVar(char ch)
        {
            if (ch >= 'a' && ch <= 'z')
                return true;
            else if (ch >= 'A' && ch <= 'Z')
                return true;
            else if (ch >= '0' && ch <= '9')
                return true;
            else if (ch == '_')
                return true;
            else
                return false;
        }

        public static bool isNumerico(char[] valor, out bool negativo)
        {
            negativo = false;
            bool dot = false;
            for (int i = 0; i < valor.Length; i++)
            {
                if (i == 0 && valor[i] == '-') { negativo = true; continue; }
                else if (i > 0 && i < valor.Length - 1 && valor[i] == '.' && !dot) { dot = true; continue; }
                else if (!isDigito(valor[i])) return false;
            }

            return true;
        }

        public static void GarantirCapacidade(ref char[] bytes, int pos, int acrescentar_comprimento)
        {
            int novoTamanho = pos + acrescentar_comprimento;

            // If null(most case fisrt time) fill byte.
            if (bytes == null)
            {
                bytes = new char[novoTamanho];
                return;
            }

            // like MemoryStream.EnsureCapacity
            int atual = bytes.Length;
            if (novoTamanho > atual)
            {
                int num = novoTamanho;
                if (num < 256)
                {
                    num = 256;
                    FastResize(ref bytes, num);
                    return;
                }
                if (num < atual * 2)
                {
                    num = atual * 2;
                }

                FastResize(ref bytes, num);
            }
        }

        static void FastResize(ref char[] array, int novoTamanho)
        {
            if (novoTamanho < 0) throw new ArgumentOutOfRangeException(nameof(novoTamanho));

            char[] array2 = array;
            if (array2 == null)
            {
                array = new char[novoTamanho];
                return;
            }

            if (array2.Length != novoTamanho)
            {
                char[] array3 = new char[novoTamanho];
                Buffer.BlockCopy(array2, 0, array3, 0, (array2.Length > novoTamanho) ? novoTamanho : array2.Length);
                array = array3;
            }
        }
    }
}
