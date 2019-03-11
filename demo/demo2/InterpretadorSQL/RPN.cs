using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpretadorSQL
{
    public class RPN
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

        public int ComprimentoRPN { get { return _compRPN; } set { _compRPN = value; } }

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
        const char PRIO_COMMA = (char)100;              // VIRGULA
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
        readonly char[] OP_VIRGULA = new char[] { PRIO_COMMA, (char)1, ',' };
        readonly char[] STMT_SELECT = new char[] { PRIO_STMT, (char)2, 'S', 'L' };
        readonly char[] CLSL_FROM = new char[] { PRIO_STMT, (char)2, 'F', 'R' };

        readonly char[] SCAN_MULT = new char[] { '*' };
        readonly char[] SCAN_DIV = new char[] { '/' };
        readonly char[] SCAN_MOD = new char[] { '%' };
        readonly char[] SCAN_SOMA = new char[] { '+' };
        readonly char[] SCAN_SUB = new char[] { '-' };
        readonly char[] SCAN_POTENCIA = new char[] { '^' };
        readonly char[] SCAN_IGUAL = new char[] { '=', '=' };
        readonly char[] SCAN_MAIOR_EQUAL = new char[] { '>', '=' };
        readonly char[] SCAN_MAIOR = new char[] { '>' };
        readonly char[] SCAN_DIFERENTE = new char[] { '!', '=' };
        readonly char[] SCAN_MENOR_MAIOR = new char[] { '<', '>' };
        readonly char[] SCAN_MENOR_IGUAL_A = new char[] { '<', '=' };
        readonly char[] SCAN_MENOR = new char[] { '<' };
        readonly char[] SCAN_DUPLO_AND = new char[] { '&', '&' };
        readonly char[] SCAN_DUPLO_OR = new char[] { '|', '|' };
        readonly char[] SCAN_NOT = new char[] { '!' };

        public RPN()
        {
            _capacidade_posfixa = -1;
        }

        int _pos = 0;
        int _inicio = -1;
        char[] _op;

        int _logicaPos = 0;

        bool _sintaxeSelect = false;
        bool _isFuncao = false;

        bool _isScanNum = false;
        bool _isVariavel = false;
        bool _isComando = false;
        bool _isString = false;
        bool _unario_ant = false;
        bool _zero_esq = false;

        bool _op_ant = false;
        bool _op_ant_arit = false;
        bool _op_ant_rel = false;
        bool _op_ant_log = false;
        bool _op_ant_vir = false;
        char[] _ch_op_ant = new char[0];
        Stack<char[]> _pilha_Op = new Stack<char[]>();
        Queue<char[]> _fila_Str = new Queue<char[]>();
        StringBuilder _sb = new StringBuilder();
        char[] _posFix = null;
        int _posfixa_comp = 0;

        /// <summary>
        /// Converte a expressão infixa para Notação Polonesa Inversa
        /// </summary>
        /// <param name="infix">Expressão fixa</param>
        /// <param name="comp">Comprimento da Array de retorno</param>
        /// <param name="out_strings">Saída da lista de Arrays de caracteres que representa as strings escritas entre aspas no meio da expressão</param>
        /// <returns>Retorna RPN</returns>
        public char[] InfixaParaPosfixa(char[] infix, char[] infix_u, out Queue<char[]> out_strings)
        {
            _pos = 0;

            if (_capacidade_posfixa == -1)
            {
                FastResize(ref _posFix, infix.Length*2);
                _posfixa_comp = _posFix.Length;
            }

            for (int pos = 0; pos < infix.Length; pos++)
            {
                if (!_isVariavel)
                {
                    if (!_isComando)
                    {
                        if (!_isString)
                        {
                            if (infix[pos] == '\'')
                            {
                                _op_ant = false;
                                _isString = true;
                                continue;
                            }
                        }
                    }
                }

                _isFuncao = false;

                if (_isComando)
                {
                    #region Comando
                    if (pos == infix.Length - 1 || !isLetra(infix[pos + 1]))
                    {
                        pos++;
                        // Todos os comandos são considerados unários
                        // Declarações

                        if (pos - _inicio == 2)
                        {
                            #region Operador OR
                            if ((infix_u[_inicio + 0] == 'O') &&
                                (infix_u[_inicio + 1] == 'R'))
                            {
                                _op_ant = true;
                                _op_ant_log = true;
                                while (_pilha_Op.Count > 0 &&
                                _pilha_Op.Peek()[2] != '(' && PRIO_OR >= _pilha_Op.Peek()[0])
                                {
                                    // O que está no topo de pilha tem mais prioridade
                                    _op = _pilha_Op.Pop();
                                    for (int b = 0; b < _op[1]; b++)
                                        _posFix[_pos++] = _op[b + 2]; // Copia para posfixa
                                    _posFix[_pos++] = ' ';
                                }

                                _pilha_Op.Push(OP_OR);
                            }
                            #endregion
                            #region Operador IN
                            else if (
                                (infix_u[_inicio + 0] == 'I') &&
                                (infix_u[_inicio + 1] == 'N'))
                            {
                                _op_ant = true;
                                while (_pilha_Op.Count > 0 &&
                                _pilha_Op.Peek()[2] != '(' && PRIO_IN >= _pilha_Op.Peek()[0])
                                {
                                    // O que está no topo de pilha tem mais prioridade
                                    _op = _pilha_Op.Pop();
                                    for (int b = 0; b < _op[1]; b++)
                                        _posFix[_pos++] = _op[b + 2]; // Copia para posfixa
                                    _posFix[_pos++] = ' ';
                                }

                                _pilha_Op.Push(OP_IN);
                            }
                            #endregion
                        }
                        else if (pos - _inicio == 3)
                        {
                            #region Operador AND
                            if ((infix_u[_inicio + 0] == 'A') &&
                                (infix_u[_inicio + 1] == 'N') &&
                                (infix_u[_inicio + 2] == 'D'))
                            {
                                _op_ant_log = true;
                                _op_ant = true;
                                while (_pilha_Op.Count > 0 &&
                                _pilha_Op.Peek()[2] != '(' && PRIO_AND >= _pilha_Op.Peek()[0])
                                {
                                    // O que está no topo de pilha tem mais prioridade
                                    _op = _pilha_Op.Pop();
                                    for (int b = 0; b < _op[1]; b++)
                                        _posFix[_pos++] = _op[b + 2]; // Copia para posfixa
                                    _posFix[_pos++] = ' ';
                                }

                                _pilha_Op.Push(OP_AND);
                            }
                            #endregion
                            #region Operador ALL
                            else if (
                                (infix_u[_inicio + 0] == 'A') &&
                                (infix_u[_inicio + 1] == 'L') &&
                                (infix_u[_inicio + 2] == 'L'))
                            {
                                _op_ant = true;
                                while (_pilha_Op.Count > 0 &&
                                _pilha_Op.Peek()[2] != '(' && PRIO_ALL >= _pilha_Op.Peek()[0])
                                {
                                    // O que está no topo de pilha tem mais prioridade
                                    _op = _pilha_Op.Pop();
                                    for (int b = 0; b < _op[1]; b++)
                                        _posFix[_pos++] = _op[b + 2]; // Copia para posfixa
                                    _posFix[_pos++] = ' ';
                                }

                                _pilha_Op.Push(OP_ALL);
                            }
                            #endregion
                            #region Operador ANY
                            else if (
                                (infix_u[_inicio + 0] == 'A') &&
                                (infix_u[_inicio + 1] == 'N') &&
                                (infix_u[_inicio + 2] == 'Y'))
                            {
                                _op_ant = true;
                                while (_pilha_Op.Count > 0 &&
                                _pilha_Op.Peek()[2] != '(' && PRIO_ANY >= _pilha_Op.Peek()[0])
                                {
                                    // O que está no topo de pilha tem mais prioridade
                                    _op = _pilha_Op.Pop();
                                    for (int b = 0; b < _op[1]; b++)
                                        _posFix[_pos++] = _op[b + 2]; // Copia para posfixa
                                    _posFix[_pos++] = ' ';
                                }

                                _pilha_Op.Push(OP_ANY);
                            }
                            #endregion
                            #region Operador NOT
                            else if (
                                (infix_u[_inicio + 0] == 'N') &&
                                (infix_u[_inicio + 1] == 'O') &&
                                (infix_u[_inicio + 2] == 'T'))
                            {
                                _op_ant_log = true;
                                _op_ant = true;
                                while (_pilha_Op.Count > 0 &&
                                _pilha_Op.Peek()[2] != '(' && PRIO_NEGACAO >= _pilha_Op.Peek()[0])
                                {
                                    // O que está no topo de pilha tem mais prioridade
                                    _op = _pilha_Op.Pop();
                                    for (int b = 0; b < _op[1]; b++)
                                        _posFix[_pos++] = _op[b + 2]; // Copia para posfixa
                                    _posFix[_pos++] = ' ';
                                }

                                _pilha_Op.Push(OP_NEGACAO);
                            }
                            #endregion

                            else if (infix_u[_inicio] == 'C')
                            {
                                #region Função COS()
                                if (
                                    (infix_u[_inicio + 1] == 'O') &&
                                    (infix_u[_inicio + 2] == 'S'))
                                {
                                    _op_ant = true;
                                    _isFuncao = true;
                                    while (_pilha_Op.Count > 0 &&
                                    _pilha_Op.Peek()[2] != '(' && PRIO_FUNC >= _pilha_Op.Peek()[0])
                                    {
                                        // O que está no topo de pilha tem mais prioridade
                                        _op = _pilha_Op.Pop();
                                        for (int b = 0; b < _op[1]; b++)
                                            _posFix[_pos++] = _op[b + 2]; // Copia para posfixa
                                        _posFix[_pos++] = ' ';
                                    }

                                    _pilha_Op.Push(FUN_COS);
                                }
                                #endregion
                            }
                            else if (infix_u[_inicio] == 'S')
                            {
                                #region Função SIN()
                                if (
                                    (infix_u[_inicio + 1] == 'I') &&
                                    (infix_u[_inicio + 2] == 'N'))
                                {
                                    _op_ant = true;
                                    _isFuncao = true;
                                    while (_pilha_Op.Count > 0 &&
                                    _pilha_Op.Peek()[2] != '(' && PRIO_FUNC >= _pilha_Op.Peek()[0])
                                    {
                                        // O que está no topo de pilha tem mais prioridade
                                        _op = _pilha_Op.Pop();
                                        for (int b = 0; b < _op[1]; b++)
                                            _posFix[_pos++] = _op[b + 2]; // Copia para posfixa
                                        _posFix[_pos++] = ' ';
                                    }

                                    _pilha_Op.Push(FUN_SIN);
                                }
                                #endregion

                                #region Função SUM()
                                else if (
                                    (infix_u[_inicio + 1] == 'U') &&
                                    (infix_u[_inicio + 2] == 'M'))
                                {
                                    _op_ant = true;
                                    _isFuncao = true;
                                    while (_pilha_Op.Count > 0 &&
                                    _pilha_Op.Peek()[2] != '(' && PRIO_FUNC >= _pilha_Op.Peek()[0])
                                    {
                                        // O que está no topo de pilha tem mais prioridade
                                        _op = _pilha_Op.Pop();
                                        for (int b = 0; b < _op[1]; b++)
                                            _posFix[_pos++] = _op[b + 2]; // Copia para posfixa
                                        _posFix[_pos++] = ' ';
                                    }

                                    _pilha_Op.Push(FUN_SUM);
                                }
                                #endregion
                            }
                            else if (infix_u[_inicio] == 'T')
                            {
                                #region Função TAN()
                                if (
                                    (infix_u[_inicio + 1] == 'A') &&
                                    (infix_u[_inicio + 2] == 'N'))
                                {
                                    _op_ant = true;
                                    _isFuncao = true;
                                    while (_pilha_Op.Count > 0 &&
                                    _pilha_Op.Peek()[2] != '(' && PRIO_FUNC >= _pilha_Op.Peek()[0])
                                    {
                                        // O que está no topo de pilha tem mais prioridade
                                        _op = _pilha_Op.Pop();
                                        for (int b = 0; b < _op[1]; b++)
                                            _posFix[_pos++] = _op[b + 2]; // Copia para posfixa
                                        _posFix[_pos++] = ' ';
                                    }

                                    _pilha_Op.Push(FUN_TAN);
                                }
                                #endregion
                            }
                        }
                        else if (pos - _inicio == 4)
                        {
                            #region Cláusula FROM
                            if ((infix_u[_inicio + 0] == 'F') &&
                                (infix_u[_inicio + 1] == 'R') &&
                                (infix_u[_inicio + 2] == 'O') &&
                                (infix_u[_inicio + 3] == 'M'))
                            {
                                _logicaPos = 3;

                                _op_ant = true;
                                while (_pilha_Op.Count > 0 &&
                                    _pilha_Op.Peek()[2] != '(' && PRIO_STMT > _pilha_Op.Peek()[0]) // Operador Unário
                                {
                                    // O que está no topo da pilha tem mais prioridade
                                    _op = _pilha_Op.Pop();
                                    for (int b = 0; b < _op[1]; b++)
                                        _posFix[_pos++] = _op[b + 2]; // Copia para posfixa
                                    _posFix[_pos++] = ' ';
                                }

                                _pilha_Op.Push(CLSL_FROM);
                            }
                            #endregion
                        }
                        else if (pos - _inicio == 6)
                        {
                            #region Instrução SELECT
                            if ((infix_u[_inicio + 0] == 'S') &&
                                (infix_u[_inicio + 1] == 'E') &&
                                (infix_u[_inicio + 2] == 'L') &&
                                (infix_u[_inicio + 3] == 'E') &&
                                (infix_u[_inicio + 4] == 'C') &&
                                (infix_u[_inicio + 5] == 'T'))
                            {
                                _sintaxeSelect = true;
                                _logicaPos = 1;
                                _op_ant = true;
                                while (_pilha_Op.Count > 0 &&
                                    _pilha_Op.Peek()[2] != '(' && PRIO_STMT > _pilha_Op.Peek()[0]) // Operador Unário
                                {
                                    // O que está no topo da pilha tem mais prioridade
                                    _op = _pilha_Op.Pop();
                                    for (int b = 0; b < _op[1]; b++)
                                        _posFix[_pos++] = _op[b + 2]; // Copia para posfixa
                                    _posFix[_pos++] = ' ';
                                }

                                _pilha_Op.Push(STMT_SELECT);
                            }
                            #endregion
                        }
                        if (pos == infix.Length) pos--;
                        _isComando = false;
                    }
                    else if (isLetra(infix[pos])) continue;
                    else
                    {
                        throw new Exception($"'{infix[pos]}' não esperado");
                    }

                    #endregion
                }
                else if (_isVariavel)
                {
                    if (infix[pos] == ' ' || infix[pos] == ',' || infix[pos] == ')')
                    {
                        _isVariavel = false;
                    }
                    else if (isCaractereVar(infix[pos]))
                    {
                        _posFix[_pos++] = infix[pos];
                    }
                    else
                    {
                        throw new Exception("Uso incorreto de '@'. Nome de variável inválido");
                    }
                    //continue;
                }
                else if (_isString) // Converte string em variável
                {
                    #region String
                    if (infix[pos] == '\\' && infix[pos + 1] == '\'')
                    {
                        _sb.Append("\'"); pos++;
                        continue;
                    }
                    else if (infix[pos] == '\'') // '
                    {
                        _isString = false;
                        _fila_Str.Enqueue(_sb.ToString().ToCharArray());           // Aloca a string na memória

                        // Cria o identificador da string alocada
                        _posFix[_pos++] = '$'; // Símbolo String
                        _posFix[_pos++] = (char)((_fila_Str.Count - 1) + ((int)'0')); // Índice da string

                        _sb.Clear();
                        continue;
                    }

                    _sb.Append(infix[pos]);
                    continue;
                    #endregion
                }
                else if (isLetra(infix[pos]))
                {
                    if (pos == 0 || (pos > 0 && (infix[pos - 1] == ' ' || infix[pos - 1] == '(' ||
                        _op_ant_arit || _op_ant_rel /*Permite juntar operadores aritméticos e relacionais com o comando*/)))
                    {
                        if (!_isComando)
                        {
                            _isComando = true;

                            #region Restabelece op_ant
                            _op_ant_arit = false;
                            _op_ant_rel = false;
                            _op_ant_log = false;
                            _op_ant_vir = false;
                            #endregion

                            _inicio = pos;
                            continue;
                        }
                    }
                }
                else if (infix[pos] == '@')
                {
                    #region Variável
                    _op_ant = false;
                    if (pos > 0 && infix[pos - 1] != ' ') // Ex.: 12@
                    {
                        throw new Exception("Uso incorreto de '@'. Insira espaço antes de '@'");
                    }
                    else if (pos == infix.Length - 1 || infix[pos + 1] == ' ') // Ex.: @
                    {
                        throw new Exception("Uso incorreto de '@'. Insira o nome da variável");
                    }

                    #region Restabelece op_ant
                    _op_ant_arit = false;
                    _op_ant_rel = false;
                    _op_ant_log = false;
                    _op_ant_vir = false;
                    #endregion

                    _isVariavel = true;
                    _posFix[_pos++] = '@';
                    continue;
                    #endregion
                }
                else if (isNum(infix[pos])) // Checa se é número
                {
                    #region Escaneamento do número
                    _op_ant = false;
                    if (!_isScanNum) // Checa se o primeiro caractere da esquerda é um número
                    {
                        #region Restabelece op_ant
                        _op_ant_arit = false;
                        _op_ant_rel = false;
                        _op_ant_log = false;
                        _op_ant_vir = false;
                        #endregion

                        _isScanNum = true;
                        if (infix[pos] == '0') _zero_esq = true;
                        if (_pos > 0 && !_unario_ant && pos > 0 && _posFix[_pos - 1] != ' ')
                        {
                            _posFix[_pos++] = ' ';
                            _unario_ant = false;
                        }
                    }

                    if (_zero_esq && infix[pos] == '0')
                    {
                        if (pos == infix.Length - 1) // Adiciona Zero no último índice
                        {
                            _posFix[_pos++] = '0';
                            _zero_esq = false;
                        }
                        continue; // Ignora zeros a esquerda
                    }
                    _zero_esq = false;
                    _posFix[_pos++] = infix[pos];
                    continue; // Pula para o próximo número ou espaço em branco
                    #endregion
                }
                else
                {
                    #region Scanneamento final do número
                    if (_isScanNum)
                    {
                        if (infix[pos] == '.')
                        {
                            _posFix[_pos++] = '.';
                            continue;
                        }
                        if (_zero_esq) // Insere o valor Zero
                        {
                            _posFix[_pos++] = '0';
                            _zero_esq = false;
                        }
                        _posFix[_pos++] = ' ';
                        _isScanNum = false;
                    }
                    #endregion
                }

                #region Operador (
                if (infix[pos] == '(')
                {
                    _op_ant = true;
                    _pilha_Op.Push(OP_ABRE_PARENTESE); // Adiciona na pilha
                    if (pos > 0 && _pos > 0 && _posFix[_pos - 1] != ' ') _posFix[_pos++] = ' ';
                }
                #endregion
                #region Operador )
                else if (infix[pos] == ')')
                {
                    _sintaxeSelect = false;
                    _logicaPos = 0;

                    while (_pilha_Op.Count > 0 &&         // Enquanto conter elementos
                           _pilha_Op.Peek()[2] != '(')    // Desempilha e lança todos no final da posfixa até encontrar um '('
                    {
                        _op = _pilha_Op.Pop();
                        for (int b = 0; b < _op[1]; b++)
                            _posFix[_pos++] = _op[b + 2]; // Copia para posfixa
                        _posFix[_pos++] = ' ';
                    }

                    if (_pilha_Op.Count == 0)
                    {
                        throw new Exception("Expressão inválida. Abre-parêntese está faltando");
                    }

                    if (_pilha_Op.Peek()[2] == '(')
                        _pilha_Op.Pop();
                    else
                        throw new Exception("Expressão inválida. Fecha-parêntese está faltando");
                }
                #endregion
                else if (infix[pos] == ' ')
                {
                    #region Separador de Argumentos
                    if (_pos > 0)
                    {
                        if (_posFix[_pos - 1] != ' ') _posFix[_pos++] = ' ';
                    }
                    #endregion
                }
                else if (Operator(SCAN_MULT, false, ref infix, pos, PRIO_MULTIPLICACAO, OP_MULTIPLICACAO)) { }
                else if (Operator(SCAN_DIV, false, ref infix, pos, PRIO_DIVISAO, OP_DIVISAO)) { }
                else if (Operator(SCAN_MOD, false, ref infix, pos, PRIO_MODULO, OP_MODULO)) { }

                else if (infix[pos] == '+')
                {
                    #region Operador unário +
                    if (_op_ant || _pos == 0 &&            // Sucede um operador anterior ou posição inicial
                        (isNum(infix[pos + 1]) ||      // Operador precede valor numérico
                        isLetra(infix[pos + 1])))         // Operador precede variável
                    {
                        if (_unario_ant) // Já existe um operador unário anterior?
                        {
                            _posFix[_pos++] = '+'; // Insere o operador unário + após o operador unário anterior
                        }
                        else
                            // Desconsidera o operador unário + se não houver nenhum operador unário anterior
                            _unario_ant = true;
                    }
                    #endregion
                    #region Operador +
                    else
                    {
                        _ch_op_ant = new char[1];
                        _ch_op_ant[0] = '+';

                        _op_ant = true;
                        _op_ant_arit = true;
                        while (_pilha_Op.Count > 0 &&
                        _pilha_Op.Peek()[2] != '(' && PRIO_ADICAO >= _pilha_Op.Peek()[0])
                        {
                            // O que está no topo de pilha tem mais prioridade
                            _op = _pilha_Op.Pop();
                            for (int b = 0; b < _op[1]; b++)
                                _posFix[_pos++] = _op[b + 2]; // Copia para posfixa
                            _posFix[_pos++] = ' ';
                        }

                        _pilha_Op.Push(OP_ADICAO);
                    }
                    #endregion
                }
                else if (infix[pos] == '-')
                {
                    #region Operador Unário -
                    if (_op_ant || _pos == 0 &&           // Sucede um operador anterior ou posição inicial
                        (isNum(infix[pos + 1]) ||     // Operador unário precede valor numérico
                        isLetra(infix[pos + 1])))        // Operador unário precede variável
                    {
                        _unario_ant = true;
                        _posFix[_pos++] = '-'; // Insere o operador unário - no final da posfixa
                    }
                    #endregion
                    #region Operador -
                    else
                    {
                        _ch_op_ant = new char[1];
                        _ch_op_ant[0] = '-';

                        _op_ant = true;
                        _op_ant_arit = true;
                        while (_pilha_Op.Count > 0 &&
                            _pilha_Op.Peek()[2] != '(' && PRIO_SUBTRACAO >= _pilha_Op.Peek()[0])
                        {
                            // O que está no topo de pilha tem mais prioridade
                            _op = _pilha_Op.Pop();
                            for (int b = 0; b < _op[1]; b++)
                                _posFix[_pos++] = _op[b + 2]; // Copia para posfixa
                            _posFix[_pos++] = ' ';
                        }
                        _pilha_Op.Push(OP_SUBTRACAO);
                    }
                    #endregion
                }
                else if (Operator(SCAN_POTENCIA, false, ref infix, pos, PRIO_POTENCIA, OP_POTENCIA)) { }
                else if (Operator(SCAN_IGUAL, false, ref infix, pos, PRIO_IGUAL_A, OP_IGUAL_A)) { }
                else if (Operator(SCAN_MAIOR_EQUAL, false, ref infix, pos, PRIO_MAIOR_OU_IGUAL_A, OP_MAIOR_OU_IGUAL_A)) { }
                else if (Operator(SCAN_MAIOR, false, ref infix, pos, PRIO_MAIOR_QUE, OP_MAIOR_QUE)) { }
                else if (infix[pos] == '!') // NOT
                {
                    #region Operador !=
                    if ((pos + 1 < infix.Length) && infix[pos + 1] == '=') // !=
                    {
                        _ch_op_ant = new char[2];
                        _ch_op_ant[0] = '!';
                        _ch_op_ant[1] = '=';

                        _op_ant_rel = true;
                        _op_ant = true;
                        while (_pilha_Op.Count > 0 &&
                               _pilha_Op.Peek()[2] != '(' && PRIO_DIFERENTE_DE >= _pilha_Op.Peek()[0])
                        {
                            // O que está no topo de pilha tem mais prioridade
                            _op = _pilha_Op.Pop();
                            for (int b = 0; b < _op[1]; b++)
                                _posFix[_pos++] = _op[b + 2]; // Copia para posfixa
                            _posFix[_pos++] = ' ';
                        }

                        _pilha_Op.Push(OP_DIFERENTE_DE);
                        pos++;
                    }
                    #endregion
                    #region Operador unário !
                    else // !
                    {
                        _op_ant_rel = true;
                        _op_ant = true;
                        while (_pilha_Op.Count > 0 &&
                        _pilha_Op.Peek()[2] != '(' && PRIO_NEGACAO > _pilha_Op.Peek()[0])
                        {
                            // O que está no topo de pilha tem mais prioridade
                            _op = _pilha_Op.Pop();
                            for (int b = 0; b < _op[1]; b++)
                                _posFix[_pos++] = _op[b + 2]; // Copia para posfixa
                            _posFix[_pos++] = ' ';
                        }

                        _pilha_Op.Push(OP_NEGACAO);
                    }
                    #endregion
                }
                
                else if (Operator(SCAN_DIFERENTE, false, ref infix, pos, PRIO_DIFERENTE_DE, OP_DIFERENTE_DE)) { }
                else if (Operator(SCAN_MENOR_MAIOR, false, ref infix, pos, PRIO_DIFERENTE_DE, OP_DIFERENTE_DE)) { }
                else if (Operator(SCAN_MENOR_IGUAL_A, false, ref infix, pos, PRIO_MENOR_OU_IGUAL_A, OP_MENOR_OU_IGUAL_A)) { }
                else if (Operator(SCAN_MENOR, false, ref infix, pos, PRIO_MENOR_QUE, OP_MENOR_QUE)) { }
                else if (Operator(SCAN_DUPLO_AND, false, ref infix, pos, PRIO_AND, OP_AND)) { }
                else if (Operator(SCAN_DUPLO_OR, false, ref infix, pos, PRIO_OR, OP_OR)) { }
                else if (OperadorUnario(SCAN_NOT, false, ref infix, pos, PRIO_NEGACAO, OP_NEGACAO)) { }

                #region Operador ,
                else if (infix[pos] == ',')
                {
                    _op_ant = true;
                    _op_ant_vir = true;
                    while (_pilha_Op.Count > 0 &&
                    _pilha_Op.Peek()[2] != '(' && PRIO_COMMA > _pilha_Op.Peek()[0])
                    {
                        // O que está no topo de pilha tem mais prioridade
                        _op = _pilha_Op.Pop();
                        for (int b = 0; b < _op[1]; b++)
                            _posFix[_pos++] = _op[b + 2]; // Copia para posfixa
                        _posFix[_pos++] = ' ';
                    }

                    _pilha_Op.Push(OP_VIRGULA);
                }
                #endregion
                else if (infix[pos] == ';') break;
                //else
                //{
                //    if (isVariavel) continue;
                //    throw new Exception("'" + new string(infixa, i, 1) + "' não esperado");
                //}
            }

            while (_pilha_Op.Count > 0)
            {
                if (_pos > 0 && _posFix[_pos - 1] != ' ') _posFix[_pos++] = ' ';
                if (_pilha_Op.Peek()[2] == '(')
                {
                    throw new Exception("Expressão inválida. Fecha-parêntese está faltando");
                }
                _op = _pilha_Op.Pop();
                for (int b = 0; b < _op[1]; b++) _posFix[_pos++] = _op[b + 2];
            }

            if (_op_ant_arit)
            {
                throw new Exception($"Erro de expressão aritmética. É esperado um valor do lado direito do operador {new string(_ch_op_ant)}");
            }
            else if (_op_ant_log)
            {
                throw new Exception($"Erro de expressão lógica. É esperado um valor do lado direito do operador {new string(_ch_op_ant)}");
            }
            else if (_op_ant_rel)
            {
                throw new Exception($"Erro de expressão relacional. É esperado um valor do lado direito do operador {new string(_ch_op_ant)}");
            }
            else  if (_sintaxeSelect && _logicaPos == 1)
            {
                if (_op_ant_vir)
                {
                    throw new Exception("Erro de sintaxe. Era esperado uma expressão ou nome da coluna após a vírgula");
                }

                //throw new Exception("Erro de sintaxe. É esperado uma expressão ou nome de coluna");
            }

            _compRPN = _pos;
            out_strings = _fila_Str;
            return _posFix;
        }

        private bool Operator(char[] op, bool naoPermitirEspacoBracoAposOp, ref char[] infixa, int pos, char precedencia, char[] empilha_op)
        {
            // Posição somada ao comprimento do operador é maior que o comprimento total do infixa?
            if ((pos + op.Length) - 1 > infixa.Length - 1) 
            {
                return false;
            }

            // Checa se este operador consta na infixa na posição atual
            for (int pos_op = 0; pos_op < op.Length; pos_op++)
            {
                if (infixa[pos + pos_op] != op[pos_op] || pos + pos_op > infixa.Length) return false;

                else if (pos_op == op.Length - 1)
                {
                    if (pos + pos_op == infixa.Length - 1)
                    {
                        throw new Exception($"Era esperado um valor do lado direito do operador '{new string(op)}'");
                    }
                    else 
                    if (naoPermitirEspacoBracoAposOp && infixa[pos + pos_op] == ' ') return false;    // Caractere após o operador não é um espaço em branco
                }
            }

            _ch_op_ant = op;     // Salva o último operador escaneado
            _op_ant_rel = true;  // Tipo de operador
            _op_ant = true;      // Operador anterior escaneado

            while (_pilha_Op.Count > 0 &&
                _pilha_Op.Peek()[2] != '(' && precedencia >= _pilha_Op.Peek()[0])
            {
                op = _pilha_Op.Pop();
                for (int b = 0; b < op[1]; b++) _posFix[this._pos++] = op[b + 2];
                _posFix[this._pos++] = ' ';
            }

            _pilha_Op.Push(empilha_op);
            pos++;

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

            _ch_op_ant = op;     // Salva o último operador escaneado
            _op_ant_rel = true;  // Tipo de operador
            _op_ant = true;      // Operador anterior escaneado

            while (_pilha_Op.Count > 0 &&
                _pilha_Op.Peek()[2] != '(' && precedencia > _pilha_Op.Peek()[0])
            {
                op = _pilha_Op.Pop();
                for (int b = 0; b < op[1]; b++) _posFix[_pos++] = op[b + 2];
                _posFix[_pos++] = ' ';
            }

            _pilha_Op.Push(empilha_op);
            i++;

            return true;
        }

        /// <summary>
        /// Calculadora de Notação Polonesa Inversa
        /// </summary>
        /// <param name="pos_fixa"></param>
        /// <param name="in_strings"></param>
        /// <param name="in_variaveis"></param>
        /// <param name="out_variaveis"></param>
        /// <returns></returns>
        //public unsafe object CalcularRPN(char* rpn, Queue<char[]> in_strings,
        //variavel[] in_variaveis, out variavel[] out_variaveis)
        public unsafe object Avaliar(char[] pos_fixa, Queue<char[]> in_strings,
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
                        if (isCaractereVar(pos_fixa[i]))
                        {
                            continue;
                        }
                        else if (pos_fixa[i] == ' ' || i == _compRPN - 1)
                        {
                            nomeVar = new char[i - inicio - 1];
                            for (int b = inicio + 1, c = 0; b < i; b++, c++)
                            {
                                nomeVar[c] = pos_fixa[b];
                            }
                            isVariavel = false;
                        }
                        else
                        {
                            throw new Exception($"'{pos_fixa[i]}' não esperado");
                        }
                    }
                    else if (isString)
                    {
                        if (isNum(pos_fixa[i]))
                        {
                            continue;
                        }
                        else if (pos_fixa[i] == ' ' || i == _compRPN - 1)
                        {
                            #region Copia a string para dblArr
                            elemento = (int)CustomToDecimal(pos_fixa, inicio + 1, i - 1, false);

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
                            throw new Exception($"'{pos_fixa[i]}' não esperado");
                        }
                    }
                    else if (isNum(pos_fixa[i]))
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
                            if (pos_fixa[i] == '.') continue;

                            _dblArr[pilhaPos++] = (double)CustomToDecimal(pos_fixa, inicio, i - 1, isNegativo);
                            isNumero = false;
                            isNegativo = false;
                        }
                    }

                    #region Variáveis e Strings
                    if (pos_fixa[i] == ' ')
                    {
                        continue;
                    }
                    else if (pos_fixa[i] == '@')
                    {
                        if (!isVariavel)
                        {
                            isVariavel = true;
                            inicio = i;
                            continue;
                        }
                    }
                    else if (pos_fixa[i] == '$')
                    {
                        if (!isString)
                        {
                            isString = true;
                            inicio = i;
                            continue;
                        }
                    }
                    else if (pos_fixa[i] == ',')
                    {
                        parametros++;
                        continue;
                    }
                    #endregion
                    #region Operadores Aritméticos
                    else if (pos_fixa[i] == '*')
                    {
                        _dblArr[pilhaPos - 2] = _dblArr[pilhaPos - 2] * _dblArr[pilhaPos - 1];
                        pilhaPos -= 1;
                    }
                    else if (pos_fixa[i] == '/')
                    {
                        _dblArr[pilhaPos - 2] = _dblArr[pilhaPos - 2] / _dblArr[pilhaPos - 1];
                        pilhaPos -= 1;
                    }
                    else if (pos_fixa[i] == '-')
                    {
                        if (isNum(pos_fixa[i + 1]))
                            isNegativo = true;  // Número negativo
                        else
                        {
                            _dblArr[pilhaPos - 2] = _dblArr[pilhaPos - 2] - _dblArr[pilhaPos - 1];  // Subtração
                            pilhaPos -= 1;
                        }
                    }
                    else if (pos_fixa[i] == '+')
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
                    else if (pos_fixa[i] == '^')
                    {
                        _dblArr[pilhaPos - 2] = Math.Pow(_dblArr[pilhaPos - 2], _dblArr[pilhaPos - 1]);
                        pilhaPos -= 1;
                    }
                    else if (pos_fixa[i] == '%')
                    {
                        _dblArr[pilhaPos - 2] = _dblArr[pilhaPos - 2] % _dblArr[pilhaPos - 1];
                        pilhaPos -= 1;
                    }
                    #endregion
                    #region Operadores Relacionais
                    else if (pos_fixa[i] == '=') // '=
                    {
                        boolExp = double.Equals(_dblArr[pilhaPos - 2], _dblArr[pilhaPos - 1]);
                        _dblArr[pilhaPos - 2] = *((byte*)(&boolExp));
                        pilhaPos -= 1;
                    }
                    else if (pos_fixa[i] == '<')
                    {
                        if (pos_fixa[i + 1] == '>') // <>
                        {
                            boolExp = !double.Equals(_dblArr[pilhaPos - 2], _dblArr[pilhaPos - 1]);
                            i++;
                        }
                        else if (pos_fixa[i + 1] == '=') // <=
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
                    else if (pos_fixa[i] == '>')
                    {
                        if (pos_fixa[i + 1] == '=') // >=
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
                    else if (pos_fixa[i] == '&')
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
                    else if (pos_fixa[i] == '|')
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
                    else if (pos_fixa[i] == '!')
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
                    else if (pos_fixa[i] == 'c' || pos_fixa[i] == 'C')
                    {
                        #region CO - COS
                        if (pos_fixa[i + 1] == 'o' || pos_fixa[i + 1] == 'O') // CO (COS)
                        {
                            if (pos_fixa[i + 2] == ' ' || i + 2 == _compRPN)
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
                    else if (pos_fixa[i] == 'f' || pos_fixa[i] == 'F')
                    {
                        #region FR - FROM
                        if (pos_fixa[i + 1] == 'r' || pos_fixa[i + 1] == 'R')
                        {
                            if (pos_fixa[i + 2] == ' ' || i + 2 == _compRPN)
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
                    else if (pos_fixa[i] == 's' || pos_fixa[i] == 'S')
                    {
                        #region SL - SELECT
                        if (pos_fixa[i + 1] == 'l' || pos_fixa[i + 1] == 'L') // SL (SELECT)
                        {
                            

                            if (pos_fixa[i + 2] == ' ' || i + 2 == _compRPN)
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
                        else if (pos_fixa[i + 1] == 'u' || pos_fixa[i + 1] == 'U') // SU (SUM)
                        {
                            if (pos_fixa[i + 2] == ' ' || i + 2 == _compRPN)
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
                        else if (pos_fixa[i + 1] == 'i' || pos_fixa[i + 1] == 'I') // SI (SIN)
                        {
                            if (pos_fixa[i + 2] == ' ' || i + 2 == _compRPN)
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
                    else if (pos_fixa[i] == 't' || pos_fixa[i] == 'T')
                    {
                        #region TG - TAN
                        if (pos_fixa[i + 1] == 'g' || pos_fixa[i + 1] == 'G') // TG (TAN)
                        {
                            if (pos_fixa[i + 2] == ' ' || i + 2 == _compRPN)
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
                        throw new Exception($"'{pos_fixa[i]}' não esperado");
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

        public double Sum(params double[] p)
        {
            double ret = 0D;
            for (int i = 0; i < p.Length; i++)
            {
                ret += p[i];
            }
            return ret;
        }

        public unsafe decimal CustomToDecimal(char[] chArray, int offset, int max_index, bool negative = false)
        {
            //public static unsafe decimal CustomToDecimal(char* chArray, int offset, int max, bool negative = false)
            long n = 0;
            int count = (max_index - offset) + 1;
            int decimalPosition = count;
            bool dot = false;
            for (int pos = offset; pos <= max_index; pos++)
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

        public static bool isNum(char ch) => ch >= '0' && ch <= '9';

        //public static bool isString(char[] valor)
        //{
        //    return (valor[0] == '\'') && valor[valor.Length - 1] == '\'';
        //}

        public bool isLetra(char ch)
        {
            if (ch >= 'a' && ch <= 'z')
                return true;
            else if(ch >= 'A' && ch <= 'Z')
                return true;
            else
                return false;
        }

        public bool isCaractereVar(char ch)
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

        public bool isNumerico(char[] valor, out bool negativo)
        {
            negativo = false;
            bool dot = false;
            for (int i = 0; i < valor.Length; i++)
            {
                if (i == 0 && valor[i] == '-') { negativo = true; continue; }
                else if (i > 0 && i < valor.Length - 1 && valor[i] == '.' && !dot) { dot = true; continue; }
                else if (!isNum(valor[i])) return false;
            }

            return true;
        }

        public void GarantirCapacidade(ref char[] bytes, int pos, int acrescentar_comprimento)
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

        public static void FastResize(ref char[] array, int novoTamanho)
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
