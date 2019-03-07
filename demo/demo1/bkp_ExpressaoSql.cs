using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotacaoPolonesaInversa
{
    public class bkp_ExpressaoSql
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
        const char PRIO_IN = (char)20;                  // IN
        const char PRIO_LIKE = (char)20;                // LIKE
        const char PRIO_OR = (char)20;                  // OR
        const char PRIO_SOME = (char)20;                // SOME

        readonly char[] OP_ABRE_PARENTESE = new char[] { PRIO_PARENTESE, (char)1, '(' };
        readonly char[] OP_NEGACAO = new char[] { PRIO_NEGACAO, (char)1, '!' };
        readonly char[] OP_POTENCIA = new char[] { PRIO_POTENCIA, (char)1, '^' };
        readonly char[] OP_MULTIPLICACAO = new char[] { PRIO_MULTIPLICACAO, (char)1, '*' };
        readonly char[] OP_DIVISAO = new char[] { PRIO_DIVISAO, (char)1, '/' };
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

        public bkp_ExpressaoSql()
        {
            _capacidade_posfixa = -1;
        }

        /// <summary>
        /// Converte a expressão infixa para Notação Polonesa Inversa
        /// </summary>
        /// <param name="infixa">Expressão em linguagem humana com parênteses, números, strings, variáveis, operadores aritméticos, relacionais e lógicos</param>
        /// <param name="comp">Comprimento da Array de retorno</param>
        /// <param name="out_strings">Saída da lista de Arrays de caracteres que representa as strings escritas entre aspas no meio da expressão</param>
        /// <returns>Retorna RPN</returns>
        public char[] InfixaParaPosfixa(char[] infixa, out Queue<char[]> out_strings)
        {
            Stack<char[]> pilha_Op = new Stack<char[]>();
            Queue<char[]> fila_Str = new Queue<char[]>();
            StringBuilder sb = new StringBuilder();

            int pos = 0;
            char[] op;

            bool op_ant = false;
            bool isNumero = false;
            bool isVariavel = false;
            bool isString = false;
            bool unario_ant = false;
            bool zero_esq = false;

            char[] posfixa = null;
            int posfixa_comp = 0;

            if (_capacidade_posfixa == -1)
            {
                FastResize(ref posfixa, infixa.Length*2);
                posfixa_comp = posfixa.Length;
            }

            for (int i = 0; i < infixa.Length; i++)
            {
                if (!isVariavel)
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

                if (isVariavel)
                {
                    if (infixa[i] == ' ')
                    {
                        isVariavel = false;
                    }
                    else if (isCaractereVar(infixa[i]))
                    {
                        posfixa[pos++] = infixa[i];
                    }
                    else
                    {
                        throw new Exception("Uso incorreto de '@'. Formato de nome de variável inválido");
                    }
                    continue;
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
                        throw new Exception("Expressão inválida. Abre-parêntese não encontrado.");
                    }

                    if (pilha_Op.Peek()[2] == '(')
                        pilha_Op.Pop();
                    else
                        throw new Exception("Expressão inválida. Fecha-parêntese não encontrado.");
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
                else if (infixa[i] == '*')
                {
                    #region Operador *

                    op_ant = true;
                    while (pilha_Op.Count > 0 &&
                        pilha_Op.Peek()[2] != '(' && PRIO_MULTIPLICACAO >= pilha_Op.Peek()[0])
                    {
                        // O que está no topo de pilha tem mais prioridade
                        op = pilha_Op.Pop();
                        for (int b = 0; b < op[1]; b++)
                            posfixa[pos++] = op[b + 2]; // Copia para posfixa
                        posfixa[pos++] = ' ';
                    }

                    pilha_Op.Push(OP_MULTIPLICACAO);
                    #endregion
                }
                else if (infixa[i] == '/')
                {
                    #region Operador /

                    op_ant = true;
                    while (pilha_Op.Count > 0 &&
                        pilha_Op.Peek()[2] != '(' && PRIO_DIVISAO >= pilha_Op.Peek()[0])
                    {
                        // O que está no topo de pilha tem mais prioridade
                        op = pilha_Op.Pop();
                        for (int b = 0; b < op[1]; b++)
                            posfixa[pos++] = op[b + 2]; // Copia para posfixa
                        posfixa[pos++] = ' ';
                    }

                    pilha_Op.Push(OP_DIVISAO);
                    #endregion
                }
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

                        op_ant = true;
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

                        op_ant = true;
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
                else if (infixa[i] == '^')
                {
                    #region Operador ^

                    op_ant = true;
                    while (pilha_Op.Count > 0 &&
                        pilha_Op.Peek()[2] != '(' && PRIO_POTENCIA >= pilha_Op.Peek()[0])
                    {
                        // O que está no topo de pilha tem mais prioridade
                        op = pilha_Op.Pop();
                        for (int b = 0; b < op[1]; b++)
                            posfixa[pos++] = op[b + 2]; // Copia para posfixa
                        posfixa[pos++] = ' ';
                    }
                    pilha_Op.Push(OP_POTENCIA);
                    #endregion
                }
                else if (infixa[i] == '=')
                {
                    #region Operador ==
                    if ((i + 1 < infixa.Length) && infixa[i + 1] == '=') i++;
                    #endregion
                    #region Operador =

                    op_ant = true;
                    while (pilha_Op.Count > 0 &&
                        pilha_Op.Peek()[2] != '(' && PRIO_IGUAL_A >= pilha_Op.Peek()[0])
                    {
                        // O que está no topo de pilha tem mais prioridade
                        op = pilha_Op.Pop();
                        for (int b = 0; b < op[1]; b++)
                            posfixa[pos++] = op[b + 2]; // Copia para posfixa
                        posfixa[pos++] = ' ';
                    }

                    pilha_Op.Push(OP_IGUAL_A);
                    #endregion
                }
                else if (infixa[i] == '>')
                {
                    #region Operador >=
                    op_ant = true;
                    if ((i + 1 < infixa.Length) && infixa[i + 1] == '=') // >=
                    {
                        while (pilha_Op.Count > 0 &&
                            pilha_Op.Peek()[2] != '(' && PRIO_MAIOR_OU_IGUAL_A >= pilha_Op.Peek()[0])
                        {
                            // O que está no topo de pilha tem mais prioridade
                            op = pilha_Op.Pop();
                            for (int b = 0; b < op[1]; b++)
                                posfixa[pos++] = op[b + 2]; // Copia para posfixa
                            posfixa[pos++] = ' ';
                        }

                        pilha_Op.Push(OP_MAIOR_OU_IGUAL_A);
                        i++;
                    }
                    #endregion
                    #region Operador >
                    else // >
                    {

                        op_ant = true;
                        while (pilha_Op.Count > 0 &&
                        pilha_Op.Peek()[2] != '(' && PRIO_MAIOR_QUE >= pilha_Op.Peek()[0])
                        {
                            // O que está no topo de pilha tem mais prioridade
                            op = pilha_Op.Pop();
                            for (int b = 0; b < op[1]; b++)
                                posfixa[pos++] = op[b + 2]; // Copia para posfixa
                            posfixa[pos++] = ' ';
                        }

                        pilha_Op.Push(OP_MAIOR_QUE);
                    }
                    #endregion
                }
                else if (infixa[i] == '!') // NOT
                {
                    #region Operador !=
                    if ((i + 1 < infixa.Length) && infixa[i + 1] == '=') // !=
                    {
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
                else if (infixa[i] == '<')
                {
                    #region Operador <>
                    if ((i + 1 < infixa.Length) && infixa[i + 1] == '>') // <>
                    {
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
                    #region Operador <=
                    else if ((i + 1 < infixa.Length) && infixa[i + 1] == '=') // <=
                    {
                        op_ant = true;
                        while (pilha_Op.Count > 0 &&
                        pilha_Op.Peek()[2] != '(' && PRIO_MENOR_OU_IGUAL_A >= pilha_Op.Peek()[0])
                        {
                            // O que está no topo de pilha tem mais prioridade
                            op = pilha_Op.Pop();
                            for (int b = 0; b < op[1]; b++)
                                posfixa[pos++] = op[b + 2]; // Copia para posfixa
                            posfixa[pos++] = ' ';
                        }

                        pilha_Op.Push(OP_MENOR_OU_IGUAL_A);
                        i++;
                    }
                    #endregion
                    #region Operador <
                    else // <
                    {
                        op_ant = true;
                        while (pilha_Op.Count > 0 &&
                        pilha_Op.Peek()[2] != '(' && PRIO_MENOR_QUE >= pilha_Op.Peek()[0])
                        {
                            // O que está no topo de pilha tem mais prioridade
                            op = pilha_Op.Pop();
                            for (int b = 0; b < op[1]; b++)
                                posfixa[pos++] = op[b + 2]; // Copia para posfixa
                            posfixa[pos++] = ' ';
                        }

                        pilha_Op.Push(OP_MENOR_QUE);
                    }
                    #endregion
                }
                #region Operador &&
                else if ((i + 1 < infixa.Length) && infixa[i] == '&')
                {
                    if (infixa[i + 1] == '&')
                    {
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
                        i++;
                    }
                    else
                    {
                        throw new NotImplementedException("Operador '&' não implementado");
                    }
                }
                #endregion
                #region Operador ||
                else if ((i + 1 < infixa.Length) && infixa[i] == '|')
                {
                    if (infixa[i + 1] == '|')
                    {
                        op_ant = true;
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
                        i++;
                    }
                    else
                    {
                        throw new NotImplementedException("Operador '|' não implementado");
                    }
                }
                #endregion
                
                else if ((i + 1 < infixa.Length) &&
                    ((i == 0 || (i > 0 && infixa[i - 1] == ' ')) && (infixa[i] == 'a' || infixa[i] == 'A'))) // 'A'
                {
                    #region Operador AND
                    if ((i + 2 < infixa.Length) && (infixa[i + 1] == 'n' || infixa[i + 1] == 'N'))      // 'AN'
                    {
                        if ((i + 3 < infixa.Length) && (infixa[i + 2] == 'd' || infixa[i + 2] == 'D'))  // 'AND'
                        {
                            if (((i + 4 < infixa.Length) && infixa[i + 3] == ' ') ||                    // 'AND '
                                i == infixa.Length - 1) // Último índice
                            {
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
                                i += 3;
                            }
                        }
                    }
                    #endregion
                    #region Operador ALL
                    else if ((i + 2 < infixa.Length) && (infixa[i + 1] == 'l' || infixa[i + 1] == 'L')) // 'AL'
                    {
                        if ((i + 3 < infixa.Length) && (infixa[i + 2] == 'l' || infixa[i + 2] == 'L'))  // 'ALL'
                        {
                            if (((i + 4 < infixa.Length) && infixa[i + 3] == ' ') ||                    // 'ALL '
                                i == infixa.Length - 1) // Último índice
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
                                i += 3;

                                throw new NotImplementedException("Implementar operador ALL");
                            }
                        }
                    }
                    #endregion
                }
                #region Operador OR
                else if ((i + 1 < infixa.Length) &&
                    ((i == 0 || (i > 0 && infixa[i - 1] == ' ')) && (infixa[i] == 'o' || infixa[i] == 'O')))          // 'O'
                {
                    if ((i + 2 < infixa.Length) && (infixa[i + 1] == 'r' || infixa[i + 1] == 'R'))      // 'OR'
                    {
                        if (((i + 3 < infixa.Length) && infixa[i + 2] == ' ') ||                        // 'OR '
                            i == infixa.Length - 1) // Último índice
                        {
                            op_ant = true;
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
                            i += 2;

                        }
                    }
                }
                #endregion
                #region Operador NOT
                else if ((i + 1 < infixa.Length) &&
                    ((i == 0 || (i > 0 && infixa[i - 1] == ' ')) && (infixa[i] == 'n' || infixa[i] == 'N')))          // 'N'
                {
                    if ((i + 2 < infixa.Length) && (infixa[i + 1] == 'o' || infixa[i + 1] == 'O'))      // 'NO'
                    {
                        if ((i + 3 < infixa.Length) && (infixa[i + 2] == 't' || infixa[i + 2] == 'T'))  // 'NOT'
                        {
                            if (((i + 3 < infixa.Length) && infixa[i + 3] == ' ') ||                    // 'NOT '
                            i == infixa.Length - 1) // Último índice
                            {
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
                                i += 3;
                            }
                        }
                    }
                }
                #endregion
                #region Operador IN
                else if ((i + 1 < infixa.Length) &&
                    ((i == 0 || (i > 0 && infixa[i - 1] == ' ')) && (infixa[i] == 'i' || infixa[i] == 'I')))    // 'I'
                {
                    if ((i + 2 < infixa.Length) && (infixa[i + 1] == 'n' || infixa[i + 1] == 'N'))              // 'IN'
                    {
                        if (((i + 3 < infixa.Length) && infixa[i + 2] == ' ') ||                                // 'IN '
                            i == infixa.Length - 1) // Último índice
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
                            i += 2;

                            throw new NotImplementedException("Implementar operador IN");
                        }
                    }
                }
                #endregion
                else
                {
                    throw new Exception("'" + new string(infixa, i, 1) + "' não esperado");
                }
            }

            while (pilha_Op.Count > 0)
            {
                if (pos > 0 && posfixa[pos - 1] != ' ') posfixa[pos++] = ' ';
                if (pilha_Op.Peek()[2] == '(')
                {
                    throw new Exception("Expressão inválida. Fecha-parêntese não encontrado");
                }
                op = pilha_Op.Pop();
                for (int b = 0; b < op[1]; b++) posfixa[pos++] = op[b + 2];
            }

            _compRPN = pos;
            out_strings = fila_Str;
            return posfixa;
        }

        /// <summary>
        /// Calculadora de Notação Polonesa Inversa
        /// </summary>
        /// <param name="rpn"></param>
        /// <param name="in_strings"></param>
        /// <param name="in_variaveis"></param>
        /// <param name="out_variaveis"></param>
        /// <returns></returns>
        public unsafe object CalcularRPN(char* rpn, Queue<char[]> in_strings,
            variavel[] in_variaveis, out variavel[] out_variaveis)
        {
            out_variaveis = null;

            byte tipo = 0;

            bool isVariavel = false;
            bool isString = false;
            bool isNumero = false;
            bool isNegativo = false;
            int inicio = -1;
            int elemento = -1;
            char[] strArr;
            char[] nomeVar;
            bool boolExp = false;

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
                                        _dblArr[pilhaPos] = (double)CustomToDecimal(pStr, isNegativo ? 1 : 0, strArr.Length - 1, isNegativo);
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

                    if (rpn[i] == ' ') continue;
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
                    else
                    {
                        throw new Exception($"'{rpn[i]}' não esperado");
                    }
                    #endregion
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

        public static unsafe decimal CustomToDecimal(char* chArray, int offset, int max, bool negative = false)
        {
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

        public static bool isString(char[] valor)
        {
            return (valor[0] == '\'') && valor[valor.Length - 1] == '\'';
        }

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
