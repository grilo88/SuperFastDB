using Interpretador;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZeroFormatter;
using static Interpretador.RPN;

namespace SuperFast
{
    public partial class frmDemo : Form
    {
        int regAtual = 0;

        Header[] header_host;
        byte[] dados_host;
        int posDadosIndex = 0;
        int posHeaderIndex = 0;

        public frmDemo()
        {
            InitializeComponent();
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
        }

        public struct exp_val
        {
            public int pos;
            public bool multDiv;
            public string metodo;
            public string coluna;
            public string operador;
            public string val;
            public string resultado_metodo;
            public int nivel;
        }

        private void btnLoadData_Click(object sender, EventArgs e)
        {
            header_host = null;
            dados_host = null;
            GC.Collect();

            int tick = 0;

            MySqlConnectionStringBuilder sb = new MySqlConnectionStringBuilder();
            sb.Server = "localhost";
            sb.UserID = "root";
            sb.Password = "123456";
            sb.MinimumPoolSize = 0;
            sb.MaximumPoolSize = 15;
            sb.ConnectionLifeTime = 10;
            sb.ConnectionProtocol = MySqlConnectionProtocol.Tcp;
            sb.ConnectionTimeout = 99999999;

            int step = 50000;

            int codDe = 0;
            int codAte = step;

            ProximoLote:
            bool continuar = false;

            using (MySqlConnection con = new MySqlConnection(sb.ToString()))
            using (MySqlCommand com = new MySqlCommand("", con))
            {
                con.Open();
                com.CommandText = "Select Codigo, Nome, Cpf, NomeMae, Sexo, Nascimento From datasintese.pfs " +
                    $"Where Codigo >= {codDe} AND Codigo < {codAte}";

                using (MySqlDataReader dr = com.ExecuteReader(CommandBehavior.Default))
                {
                    posDadosIndex = 0;
                    posHeaderIndex = 0;

                    #region Obtém o comprimento estimado dos dados
                    DataTable esquema = dr.GetSchemaTable();

                    int quantRows = step;
                    int quantCols = esquema.Rows.Count;
                    int headerQuant = quantRows * quantCols;
                    header_host = new Header[headerQuant];

                    int totalRowSize = 0;
                    foreach (DataRow r in esquema.Rows)
                    {
                        if ((Type)r["DataType"] == typeof(string))
                        {
                            totalRowSize += (int)r["ColumnSize"];
                        }
                        else if ((Type)r["DataType"] == typeof(int))
                            totalRowSize += sizeof(int);
                        else if ((Type)r["DataType"] == typeof(long))
                            totalRowSize += sizeof(long);
                        else if ((Type)r["DataType"] == typeof(DateTime))
                            totalRowSize += sizeof(long);
                        else
                            throw new NotSupportedException($"{((Type)r["DataType"]).GetType().Name} não suportado!");
                    }
                    int sizeDados = totalRowSize * quantRows;
                    dados_host = new byte[sizeDados];
                    #endregion

                    tick = Environment.TickCount;

                    #region Obtém os dados
                    int row = -1;

                    while (dr.Read())
                    {
                        continuar = true;
                        row++;
                        for (int col = 0; col < dr.FieldCount; col++)
                        {
                            object valor = dr.GetValue(col);
                            AddValue(row, col, valor);
                        }

                        regAtual++;
                        Application.DoEvents();
                    }
                    #endregion

                    #region Redimensiona os buffers para tamanhos corretos
                    Array.Resize(ref header_host, posHeaderIndex);
                    Array.Resize(ref dados_host, posDadosIndex);
                    #endregion
                }
            }

            int tempoGasto = Environment.TickCount - tick;

            if (continuar)
            {
                codDe = codAte + 1;
                codAte += step;

                goto ProximoLote;
            }

        }
        void AddValue(int row, int col, object valor)
        {
            byte[] buffer = null;
            int type = -1;
            int count = -1;

            if (valor is DBNull)
            {
                type = 0; // DBNULL
                buffer = new byte[0];
                count = buffer.Length;
            }
            else if (valor is string)
            {
                type = 1; // String
                buffer = ASCIIEncoding.ASCII.GetBytes((string)valor);
                count = buffer.Length;
            }
            else if (valor is int)
            {
                type = 2; // Int32
                buffer = BitConverter.GetBytes((int)valor);
                count = 4;
            }
            else if (valor is long)
            {
                type = 3; // Int64
                buffer = BitConverter.GetBytes((long)valor);
                count = 8;
            }
            else if (valor is DateTime)
            {
                type = 4; // Int64
                buffer = BitConverter.GetBytes(((DateTime)valor).Ticks);
                count = 8;
            }
            else
                throw new NotSupportedException($"{valor.GetType().Name} não suportado");

            header_host[posHeaderIndex].type = type;
            header_host[posHeaderIndex].index = posDadosIndex;
            header_host[posHeaderIndex].count = count;
            header_host[posHeaderIndex].row = row;
            header_host[posHeaderIndex].col = col;

            Buffer.BlockCopy(buffer, 0, dados_host, posDadosIndex, count);

            posHeaderIndex++;
            posDadosIndex += count;
        }

        private void tmr_Tick(object sender, EventArgs e)
        {
            //lblStatus.Text = "Registros Carregados na Memória: " + regAtual.ToString("N0");
        }

        private void btnCreateTable_Click(object sender, EventArgs e)
        {
            List<Cab_Coluna> colunas = new List<Cab_Coluna>();

            colunas.Add(new Cab_Coluna()
            {
                columnName = "id",
                dbType = DbType.Int32,
                size = 4,
                __stored_dataType = tipoDados.__int,
                __bytes = 4
            });
            colunas.Add(new Cab_Coluna()
            {
                columnName = "nome",
                dbType = DbType.String,
                size = 28,
                __stored_dataType = tipoDados.__string,
                __bytes = 28
            });
            colunas.Add(new Cab_Coluna()
            {
                columnName = "idade",
                dbType = DbType.Int32,
                size = 4,
                __stored_dataType = tipoDados.__int,
                __bytes = 4
            });

            CreateTableMem("tabela", colunas.ToArray());
        }

        void CreateTableMem(string nome, Cab_Coluna[] colunas)
        {
            bool bCriarCab = false;

            int tick = Environment.TickCount;
            Cab_Tabela tabela = new Cab_Tabela();
            tabela.tableName = nome;
            tabela.columns = colunas;
            colunas.Select((x, y) => new { col = x, idx = y }).ToList().ForEach(x => x.col.ordinal = x.idx);

            byte[] bytes = ZeroFormatterSerializer.Serialize(tabela);

            using (FileStream fs = new FileStream(nome, FileMode.Create))
            using (BinaryWriter bw = new BinaryWriter(fs))
            {
                bw.Write(bytes);
            }

            // Gerar Valores para a Tabela
            int quantRows = 55000000;
            int quantColunas = colunas.Count();
            int quantCampos = quantRows * colunas.Count();
            int tamanhoCol =
                4 + // row      4 bytes
                1 + // col      1 byte
                4 + // index    4 bytes
                4;  // count    4 bytes

            long tamanho_cab_dados = tamanhoCol * quantCampos;
            ulong tamanho_dados = (ulong)colunas.Sum(x => x.__bytes) * (ulong)quantRows;

            int tamanho_valores_row = colunas.Sum(x => x.__bytes);

            byte[] cab_dados = null;
            byte[] dados = new byte[tamanho_dados];

            if (bCriarCab) cab_dados = new byte[tamanho_cab_dados];

            #region Gera os valores da tabela
            string c_nome = "guilherme moreira de alencar";
            int c_idade = 30;

            int tickGerarDados = Environment.TickCount;
            Parallel.For(0, quantCampos, (campoAtual) =>
            //for (int campoAtual = 0; campoAtual < quantCampos ; campoAtual++) // Gera 'cab_valores.bin'
            {
                int posByteCampo = (campoAtual * tamanhoCol);
                int row = (campoAtual / quantColunas);
                byte col = (byte)(campoAtual - (quantColunas * row));

                #region Cria o valor
                byte[] bytes_valor = null;

                int id = row;

                if (col == 0) bytes_valor = BitConverter.GetBytes(id);
                if (col == 1) bytes_valor = ASCIIEncoding.ASCII.GetBytes(c_nome);
                if (col == 2) bytes_valor = BitConverter.GetBytes(c_idade);
                int valor_count = colunas[col].__bytes; // Coluna de tamanho fixo

                int posByteDados = row * tamanho_valores_row;
                for (int c = 0; c < col; c++)
                {
                    posByteDados += colunas[c].__bytes; // Obtém posição pela soma dos bytes
                }

                Buffer.BlockCopy(bytes_valor, 0, dados, posByteDados, bytes_valor.Length);
#warning O Buffer.BlockCopy Está ultrapassando os limites da matriz

                // Index e count para o cabeçalho do valor
                int index = posByteDados;
                int count = valor_count;
                #endregion

                #region Cria o cabeçalho do valor
                if (bCriarCab)
                {
                    byte[] bytesRow = BitConverter.GetBytes(row);
                    byte[] bytesIndex = BitConverter.GetBytes(index);
                    byte[] bytesCount = BitConverter.GetBytes(count);

                    int posByte = posByteCampo;

                    Buffer.BlockCopy(bytesRow, 0, cab_dados, posByte, bytesRow.Length);
                    posByte += bytesRow.Length;
                    cab_dados[posByte++] = col;
                    Buffer.BlockCopy(bytesIndex, 0, cab_dados, posByte, bytesIndex.Length);
                    posByte += bytesIndex.Length;
                    Buffer.BlockCopy(bytesCount, 0, cab_dados, posByte, bytesCount.Length);
                    posByte += bytesCount.Length;
                }
                #endregion
                //}
            });
            int tempoGastoGerarDados = Environment.TickCount - tickGerarDados;
            #endregion

            string path_cab_dados = nome + "_cab_dados.bin";
            string path_dados = nome + "_dados.bin";

            if (bCriarCab)
            {
                using (FileStream fs = new FileStream(path_cab_dados, FileMode.Create))
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    bw.Write(cab_dados);
                }
                cab_dados = null;
                GC.Collect();
            }

            using (FileStream fs = new FileStream(path_dados, FileMode.Create))
            using (BinaryWriter bw = new BinaryWriter(fs))
            {
                bw.Write(dados);
            }
            dados = null;
            GC.Collect();

            int tempoGasto = Environment.TickCount - tick;

            MessageBox.Show(this, $"Tabela '{nome}' criada com {quantRows.ToString("N0")} registros gerados em {tempoGasto.ToString("N0")}ms");
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            search(null);
        }

        void search(object obj)
        {
            int tick = Environment.TickCount;

            string nomeTabela = "tabela";

            Cab_Tabela tabela = null;

            using (FileStream fs = new FileStream(nomeTabela, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (BinaryReader br = new BinaryReader(fs))
            {
                int count = (int)br.BaseStream.Length;

                byte[] buffer = new byte[count];
                br.Read(buffer, 0, count);

                tabela = ZeroFormatterSerializer.Deserialize<Cab_Tabela>(buffer);
            }

            #region Realiza a Consulta Sequencial
            int consulta = 10000000;
            byte[] valor_consulta = BitConverter.GetBytes(consulta);
            byte[] select_colunas = new byte[] { 0, 1, 2 };

            select_query select_Query = new select_query();
            select_Query.table_cab = tabela;

            long encontrados = 0;
            byte[] resultado = Select_Sequencial_Colunas_Comprimento_Fixo(
                out encontrados, select_Query, select_colunas, 0, 1, tabela.columns[0].ordinal, false, op.MaiorQue, valor_consulta);
            #endregion

            int tempoGasto = Environment.TickCount - tick;

            MessageBox.Show($"{encontrados} registro(s) encontrado(s) em {tempoGasto.ToString("N0")}ms");
        }

        public struct where_exp
        {
            public bool negativo;
            public int col;
            public op op;
            public byte[] valor;
        }

        /// <summary>
        /// Realiza a consulta sequencial em tabela onde todas as colunas são de comprimento fixo
        /// </summary>
        /// <param name="tabela">Cabeçalho da tabela</param>
        /// <param name="select_colunas">Opcional: Colunas a serem capturadas do banco de dados. Por padrão todas as colunas serão retornadas.</param>
        /// <param name="bytes_consulta">Opcional: Valor da consulta em Array de Bytes idêntico ao constante no arquivo binário (Sem conversão). Por padrão nenhum filtro será utilizado.</param>
        /// <param name="col">Opcional: Coluna utilizada na cláusula Where. Por padrão nenhum filtro será utilizado.</param>
        /// <param name="op">Opcional: Operador de compração utilizada na cláusula Where. Por padrão o valor nenhum é utilizado.</param>
        /// <param name="quantRegistros">Opcional: Quantidade de registros a serem retornadas. Por padrão irá retornar todos os registros da tabela.</param>
        /// <returns></returns>
        byte[] Select_Sequencial_Colunas_Comprimento_Fixo(out long founds, select_query select_query, byte[] select_colunas = null,
            long pular = 0, long limite = -1, int col = -1, bool negacao = false, op op = op.Nenhum, byte[] valor_consulta = null)
        {
            int currentRow = -1;
            int currentCol = -1;

            long nRetornados = 0;
            byte[] resultado = new byte[0];

            string path_cab_dados = select_query.table_cab.tableName + "_cab_dados.bin";
            string path_dados = select_query.table_cab.tableName + "_dados.bin";
            FileInfo fi_cab_dados = new FileInfo(path_cab_dados);
            FileInfo fi_dados = new FileInfo(path_dados);

            long lerDadosRegistros = 250;
            long tamanho_valores_row = (long)select_query.table_cab.columns.Sum(x => x.__bytes);
            long lerDadosBytes = lerDadosRegistros * tamanho_valores_row;

            byte[] valorBanco = new byte[select_query.table_cab.columns[col].__bytes];
            object valorBancoObj = DBNull.Value;

            #region SELECT
            //using (FileStream fscd = new FileStream(path_cab_dados, FileMode.Open))
            //using (BinaryReader br_cd = new BinaryReader(fscd))
            //{
            //    // cab_dados
            //    long length_cd = br_cd.BaseStream.Length;
            //    br_cd.BaseStream.Seek(0, SeekOrigin.Begin);

            using (FileStream fsd = new FileStream(path_dados, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (BinaryReader br_d = new BinaryReader(fsd))
            {
                // dados
                long length_d = (long)fi_dados.Length;
                if (lerDadosBytes > length_d) lerDadosBytes = length_d;

                br_d.BaseStream.Seek(0, SeekOrigin.Begin);
                long posByte = 0;
                int distColBytes = 0;

                // Obtém a distância da coluna em relação ao primeiro byte do registro.
                for (long c = 0; c < (long)col; c++) distColBytes += select_query.table_cab.columns[c].__bytes;
                tipoDados col_stored_dataType = select_query.table_cab.columns[col].__stored_dataType;
                DbType col_dbType = select_query.table_cab.columns[col].dbType;

                object valorConsulta = null;
                if (col_stored_dataType == tipoDados.__int) valorConsulta = BitConverter.ToInt32(valor_consulta, 0);

                int nConjuntoDados = 0;
                long nRegistrosCarregados = 0;

                bool encerrar = false;
                while (!encerrar)
                {
                    #region Principal
                    byte[] conjunto = br_d.ReadBytes((int)lerDadosBytes);
                    nRegistrosCarregados += lerDadosRegistros;

#warning Na versão GPU deverá enviar o conjunto para a placa de Video

                    #region Percorre a coluna da cláusula Where
                    for (long i = 0; i < lerDadosRegistros; i++) // Lê somente a coluna Where
                    {
                        long index = i * tamanho_valores_row;
                        int count = select_query.table_cab.columns[col].__bytes;

                        if (index == length_d) // Atingiu o final do arquivo quando lerDadosRegistros é excedente
                        {
                            encerrar = true;
                            break;
                        }

                        currentRow = 0;
                        currentCol = col;

                        Buffer.BlockCopy(conjunto, (int)index + distColBytes, valorBanco, 0, count);

                        bool WhereSatisfeito = false;
                        WhereSatisfeito = CompareValuesOp(op, valorBanco, valor_consulta, valorConsulta, col_stored_dataType);

                        if (negacao) WhereSatisfeito = !WhereSatisfeito;
                        if (WhereSatisfeito)
                        {
                            nRetornados++;

                            #region Temp
                            if (count == 0)
                            {
                                valorBancoObj = DBNull.Value; // Quantidade Zero é Valor Nulo 
                            }
                            else if (col_dbType == DbType.Int32)
                            {
                                valorBancoObj = BitConverter.ToInt32(conjunto, (int)index);
                            }
                            else if (col_dbType == DbType.String)
                            {
                                valorBancoObj = ASCIIEncoding.ASCII.GetString(conjunto, (int)index + distColBytes, count);
                            }
                            else
                                throw new NotImplementedException("Tipo não implementado");

                            // Atingiu o limite de registros a serem retornados
                            if (limite > -1 && nRetornados == limite)
                            {
                                encerrar = true;
                                break;
                            }
                            #endregion
                        }
                    }
                    #endregion

                    #endregion

                    posByte += lerDadosBytes;
                    nConjuntoDados++;

                    if (posByte >= length_d)
                        break; // Atingiu o final do arquivo
                }

            } // dados
            //} // cab_dados
            #endregion

            founds = nRetornados;
            return resultado;
        }

        /// <summary>
        /// Compara valor da consulta com a do banco de dados determinada pelo operador.
        /// </summary>
        /// <param name="op"></param>
        /// <param name="a">Valor do banco</param>
        /// <param name="b">Valor de consulta</param>
        /// <param name="obj_a">Valor de consulta já convertido no tipo de dados. Isto evitará o uso contínuo do BitConverter e ganhará mais velocidade.</param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        bool CompareValuesOp(op op, byte[] a, byte[] b, object obj_b, tipoDados tipo)
        {
            if (obj_b == null) throw new Exception($"{nameof(obj_b)} deve ser informado");

            // IgualA é a mesma regra para todos os tipos de dados
            if (op == op.IgualA) return UnSafeEquals(a, b);

            switch (tipo)
            {
                case tipoDados.__binary:
                    if (op == op.MaiorQue) return MaiorQue(a, b);
                    else if (op == op.MenorQue) return MenorQue(a, b);
                    break;
                case tipoDados.__string:
                    throw new NotSupportedException(nameof(op));
                    break;
                case tipoDados.__char:
                    if (op == op.MaiorQue) return MaiorQue((char)a[0], (char)obj_b);
                    else if (op == op.MenorQue) return MenorQue((char)a[0], (char)obj_b);
                    break;
                case tipoDados.__sbyte:
                    if (op == op.MaiorQue) return MaiorQue((sbyte)a[0], (sbyte)obj_b);          // Verificar sbyte
                    else if (op == op.MenorQue) return MenorQue((sbyte)a[0], (sbyte)obj_b);     // Verificar sbyte
                    break;
                case tipoDados.__short:
                    if (op == op.MaiorQue) return MaiorQue(BytesToInt16(a), (short)obj_b);
                    else if (op == op.MenorQue) return MenorQue(BytesToInt16(a), (short)obj_b);
                    break;
                case tipoDados.__int24:
                    throw new NotSupportedException(nameof(op));
                    break;
                case tipoDados.__int:
                    if (op == op.MaiorQue) return MaiorQue(BytesToInt32(a), (int)obj_b);
                    else if (op == op.MenorQue) return MenorQue(BytesToInt32(a), (int)obj_b);
                    break;
                case tipoDados.__long:
                    if (op == op.MaiorQue) return MaiorQue(BytesToInt64(a), (long)obj_b);
                    else if (op == op.MenorQue) return MenorQue(BytesToInt64(a), (long)obj_b);
                    break;
                case tipoDados.__byte:
                    if (op == op.MaiorQue) return MaiorQue(a[0], (byte)obj_b);
                    else if (op == op.MenorQue) return MenorQue(a[0], (byte)obj_b);
                    break;
                case tipoDados.__ushort:
                    if (op == op.MaiorQue) return MaiorQue(BytesToUInt16(a), (ushort)obj_b);
                    else if (op == op.MenorQue) return MenorQue(BytesToUInt16(a), (ushort)obj_b);
                    break;
                case tipoDados.__uint24:
                    throw new NotSupportedException(nameof(op));
                    break;
                case tipoDados.__uint:
                    if (op == op.MaiorQue) return MaiorQue(BytesToUInt32(a), (uint)obj_b);
                    else if (op == op.MenorQue) return MenorQue(BytesToUInt32(a), (uint)obj_b);
                    break;
                case tipoDados.__ulong:
                    if (op == op.MaiorQue) return MaiorQue(BytesToUInt64(a), (ulong)obj_b);
                    else if (op == op.MenorQue) return MenorQue(BytesToUInt64(a), (ulong)obj_b);
                    break;
                case tipoDados.__float:
                    if (op == op.MaiorQue) return MaiorQue(BytesToSingle(a), (float)obj_b);
                    else if (op == op.MenorQue) return MenorQue(BytesToSingle(a), (float)obj_b);
                    break;
                case tipoDados.__double:
                    if (op == op.MaiorQue) return MaiorQue(BytesToDouble(a), (double)obj_b);
                    else if (op == op.MenorQue) return MenorQue(BytesToDouble(a), (double)obj_b);
                    break;
                case tipoDados.__decimal:
                    if (op == op.MaiorQue) return MaiorQue(BytesToDecimal(a), (decimal)obj_b);
                    else if (op == op.MenorQue) return MenorQue(BytesToDecimal(a), (decimal)obj_b);
                    break;
                default:
                    throw new NotSupportedException(nameof(op));
            }

            return false;
        }

        private bool MaiorQue(int a, int b) => a > b;
        private bool MenorQue(int a, int b) => a < b;
        private bool MaiorQue(long a, long b) => a > b;
        private bool MenorQue(long a, long b) => a < b;
        private bool MaiorQue(ulong a, ulong b) => a > b;
        private bool MenorQue(ulong a, ulong b) => a < b;
        private bool MaiorQue(short a, short b) => a > b;
        private bool MenorQue(short a, short b) => a < b;
        private bool MaiorQue(ushort a, ushort b) => a > b;
        private bool MenorQue(ushort a, ushort b) => a < b;
        private bool MaiorQue(float a, float b) => a > b;
        private bool MenorQue(float a, float b) => a < b;
        private bool MaiorQue(double a, double b) => a > b;
        private bool MenorQue(double a, double b) => a < b;
        private bool MaiorQue(decimal a, decimal b) => a > b;
        private bool MenorQue(decimal a, decimal b) => a < b;
        private bool MaiorQue(sbyte a, sbyte b) => a > b;
        private bool MenorQue(sbyte a, sbyte b) => a < b;
        private bool MaiorQue(byte a, byte b) => a > b;
        private bool MenorQue(byte a, byte b) => a < b;
        private bool MaiorQue(char a, char b) => a > b;
        private bool MenorQue(char a, char b) => a < b;
        private bool MaiorQue(byte[] a, byte[] b) => a.Length > b.Length;   // Compara comprimento
        private bool MenorQue(byte[] a, byte[] b) => a.Length < b.Length;   // Compara comprimento

        private unsafe short BytesToInt16(byte[] buffer, int pos = 0)
        {
            fixed (byte* p = &buffer[pos]) return *((short*)p);
        }
        private unsafe ushort BytesToUInt16(byte[] buffer, int pos = 0)
        {
            fixed (byte* p = &buffer[pos]) return *((ushort*)p);
        }

        private unsafe int BytesToInt32(byte[] buffer, int pos = 0)
        {
            fixed (byte* p = &buffer[pos]) return *((int*)p);
        }

        private unsafe uint BytesToUInt32(byte[] buffer, int pos = 0)
        {
            fixed (byte* p = &buffer[pos]) return *((uint*)p);
        }

        private unsafe long BytesToInt64(byte[] buffer, int pos = 0)
        {
            fixed (byte* p = &buffer[pos]) return *((long*)p);
        }

        private unsafe ulong BytesToUInt64(byte[] buffer, int pos = 0)
        {
            fixed (byte* p = &buffer[pos]) return *((ulong*)p);
        }

        private unsafe float BytesToSingle(byte[] buffer, int pos = 0)
        {
            fixed (byte* p = &buffer[pos]) return *((float*)p);
        }

        private unsafe double BytesToDouble(byte[] buffer, int pos = 0)
        {
            fixed (byte* p = &buffer[pos]) return *((double*)p);
        }

        private unsafe decimal BytesToDecimal(byte[] buffer, int pos = 0)
        {
            fixed (byte* p = &buffer[pos]) return *((decimal*)p);
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
        private unsafe bool UnSafeEquals(byte[] strA, byte[] strB)
        {
            int length = strA.Length;
            if (length != strB.Length)
            {
                return false;
            }
            fixed (byte* str = strA)
            {
                byte* chPtr = str;
                fixed (byte* str2 = strB)
                {
                    byte* chPtr2 = str2;
                    byte* chPtr3 = chPtr;
                    byte* chPtr4 = chPtr2;
                    while (length >= 10)
                    {
                        if ((((*(((int*)chPtr3)) != *(((int*)chPtr4))) || (*(((int*)(chPtr3 + 2))) != *(((int*)(chPtr4 + 2))))) || ((*(((int*)(chPtr3 + 4))) != *(((int*)(chPtr4 + 4)))) || (*(((int*)(chPtr3 + 6))) != *(((int*)(chPtr4 + 6)))))) || (*(((int*)(chPtr3 + 8))) != *(((int*)(chPtr4 + 8)))))
                        {
                            break;
                        }
                        chPtr3 += 10;
                        chPtr4 += 10;
                        length -= 10;
                    }
                    while (length > 0)
                    {
                        if (*(((int*)chPtr3)) != *(((int*)chPtr4)))
                        {
                            break;
                        }
                        chPtr3 += 2;
                        chPtr4 += 2;
                        length -= 2;
                    }
                    return (length <= 0);
                }
            }
        }

        public enum JoinType
        {
            CROSS,
            INNER,
            LEFT,
            RIGHT,
            OUTER,
        }

        public struct select_where
        {
            public string name;
            public string[] args;
        }

        public struct join_on
        {
            public op op;
            public string left_exp;
            public string rigth_exp;
        }

        public struct select_join
        {
            public JoinType type;
            public string table_name;
            public join_on[] on_exp;
        }

        public struct select_limit
        {
            public long limite;
            public long pular;
        }

        public struct select_column_query_method
        {
            public string name;
            public string[] args;
        }

        public struct select_column_query
        {
            public string column_name;
            public string view_column_name;
            public select_column_query_method[] methods;
            public int methods_count;
        }

        struct select_query
        {
            public Cab_Tabela table_cab;
            public bool distinct;
            public string table_name;
            public string alias;
            public select_column_query[] columns;
            public int columns_count;
            public select_limit limit;
        }

        private object Solicitar(string solicitacao)
        {
            int tick = Environment.TickCount;

            string[] query = solicitacao.SplitSqlQuery(new string[] {
                "<=>",
                "==", "!=", "<>", ">=", "<=", "&&", "||", ":=", // Delimitadores de maior comprimento para o menor comprimento
                " ", "(", ")", ",", "=", "\r", "\n", "{", "}", "[", "]", ";", "\t",
                ">", "<", "-", "+", "*", "/", "%", "|", "!", "&", "~", "^" }, StringSplitOptions.RemoveEmptyEntries, false).ToArray();

            int i = 0;

            ProximoComandoSql:
            select_query select_query = new select_query();
            select_query.columns = new select_column_query[255];
            select_query.limit = new select_limit();
            select_query.limit.limite = -1;
            select_column_query_method[] column_methods = null;

            int column_methods_count = 0;
            bool column_method = false;
            int nivel_colunas_select = -1;

            int logic_pos = 0;
            int nivelParentese = -1;

            bool executeSelect = false;
            bool executeCreateDataBase = false;
            bool executeCreateTable = false;
            bool executeRenameTable = false;
            bool executeTruncateTable = false;
            bool executeDropDataBase = false;
            bool executeDropTable = false;
            bool executeDeleteFrom = false;

            int select_columnPos = 0;
            bool select = false;
            bool where = false;
            bool join = false;
            bool groupby = false;
            bool orderby = false;
            bool limit = false;

            int idx_valor_proximo = -1, idx_valor_anterior = -1, idx_valor_anterior2 = -1;
            for (; i < query.Length; i++)
            {
                if (idx_valor_proximo > -1) i += (idx_valor_proximo - i); // Vai direto para a posição do próximo valor pulando espaços em branco ou vazios

                string valor = query[i];
                if (valor == ";") break; // Considera o caractere ; como fim de comando atual

                string valor_proximo = "";
                string valor_anterior = "";
                string valor_anterior2 = "";

                #region Busca os próximos valores
                for (int b = i - 1; b > -1; b--) // Obtém o primeiro valor mais próximo do lado esquerdo
                    if (query[b] != " " && query[b] != "")
                    {
                        valor_anterior = query[b];
                        idx_valor_anterior = b;
                        break;
                    }
                    else idx_valor_anterior = -1;
                for (int b = idx_valor_anterior - 1; b > -1; b--) // Obtém o segundo valor mais próximo do lado esquerdo
                    if (query[b] != " " && query[b] != "")
                    {
                        valor_anterior2 = query[b];
                        idx_valor_anterior2 = b;
                        break;
                    }
                    else idx_valor_anterior2 = -1;
                for (int b = i + 1; b < query.Length; b++) // Obtém o valor mais próximo do lado direito
                    if (query[b] != " " && query[b] != "")
                    {
                        valor_proximo = query[b];
                        idx_valor_proximo = b;
                        break;
                    }
                    else idx_valor_proximo = -1;
                #endregion

                if (valor == "(") { nivelParentese++; continue; }
                if (valor == ")") { nivelParentese--; continue; }

                if (logic_pos == 3000 && (valor == "" || valor == " ")) continue; // Pula entradas vazias após o final do comando
                if (logic_pos == 3000 && (valor != "" && valor != " ")) // Desconsidera qualquer conteúdo após o final do comando
                    throw new SqlQueryException($"'{valor}' inválido", 0);

                #region Instruções
                if (!select && valor == "SELECT")
                {
                    if (valor_proximo == valor) throw new SqlQueryException($"Cláusula '{valor}' duplicada", 0);

                    select = true;
                    executeSelect = true;
                    select_columnPos = -1;
                    logic_pos++;
                    continue;
                }
                #endregion

                #region Cláusulas
                if (where)
                {
                    if (logic_pos == 100)
                    {

                    }
                }

                if (join)
                {
                    where = false;
                    if (logic_pos == 200)
                    {
                        if (valor == "INNER" || valor == "JOIN" || valor == "LEFT" || valor == "RIGHT")
                        {
                            continue;
                        }

                        string tabela_join = valor;
                    }
                }

                if (groupby)
                {
                    join = false;
                    if (logic_pos == 300)
                    {

                    }
                }

                if (orderby)
                {
                    groupby = false;
                    if (logic_pos == 400)
                    {

                    }
                }

                if (limit)
                {
                    orderby = false;
                    if (select)
                    {
                        if (logic_pos == 502)
                        {
                            long v = -1;
                            if (!long.TryParse(valor, out v))
                            {
                                throw new SqlQueryException($"'{valor}' inválido. Era esperado o valor do limite", 0);
                            }
                            else
                            {
                                // Como tem mais um argumento então desloca o valor do limite para o pular
                                // Ex.: De LIMIT 100 para LIMIT 0, 100
                                select_query.limit.pular = select_query.limit.limite; // Pular índices
                                select_query.limit.limite = v;      // Limite de registros
                                logic_pos = 3000;
                                continue;
                            }
                        }

                        if (logic_pos == 501)
                        {
                            if (valor == ",")
                            {
                                logic_pos++; continue;
                            }
                            else
                            {
                                throw new SqlQueryException($"'{valor}' inválido. Era esperado o separador ','", 0);
                            }
                        }

                        if (logic_pos == 500)
                        {
                            long v = -1;
                            if (!long.TryParse(valor, out v))
                            {
                                throw new SqlQueryException($"'{valor}' inválido. Era esperado o valor do limite", 0);
                            }
                            else
                            {
                                select_query.limit.limite = v;  // Limite de registros
                                logic_pos++;
                                continue;
                            }

                        }

                        logic_pos = 3000;
                        continue;
                    }
                }
                #endregion

                #region Lógica Select
                if (select && (logic_pos >= 0 && logic_pos <= 6))
                {
                    Inicio_Select:
                    // 6. Identifica a cláusula Where, Group By, Order By ou Limit 
                    if (logic_pos == 6)
                    {
                        if (valor == "WHERE")
                        {
                            where = true;
                            logic_pos = 100;
                            continue;
                        }
                        else if (valor == "INNER" || valor == "JOIN" || valor == "LEFT" || valor == "RIGHT")
                        {
                            join = true;
                            logic_pos = 200;
                            continue;
                        }
                        else if (valor == "GROUP" && valor_proximo == "BY")
                        {
                            groupby = true;
                            logic_pos = 300;
                            continue;
                        }
                        else if (valor == "ORDER" && valor_proximo == "BY")
                        {
                            orderby = true;
                            logic_pos = 400;
                            continue;
                        }
                        else if (valor == "LIMIT")
                        {
                            limit = true;
                            logic_pos = 500;
                            continue;
                        }
                    }

                    // 5. Obtém o alias da tabela
                    if (logic_pos == 5)
                    {
                        select_query.alias = valor;
                        logic_pos++;
                        continue;
                    }

                    // 4. Checa a existência do alias
                    if (logic_pos == 4)
                    {
                        if (valor == "AS")
                        {
                            logic_pos++;
                            continue;
                        }
                        else
                        {
                            // O uso do alias é opcional
                            logic_pos = 6; // Pula para a lógica 6
                            goto Inicio_Select;
                        }
                    }

                    // 3. Obtém o nome da tabela da cláusula FROM
                    if (logic_pos == 3)
                    {
                        select_query.table_name = valor;
                        logic_pos++;
                        continue;
                    }

                    // 2. Identifica a cláusula FROM
                    if (logic_pos == 2 && valor == "FROM")
                    {
                        if (nivelParentese < -1) throw new SqlQueryException($"Faltou abrir parêntese antes da cláusula '{valor}'", 0);
                        if (nivelParentese > -1) throw new SqlQueryException($"Faltou fechar parêntese antes da cláusula '{valor}'", 0);
                        if (valor_proximo == valor) throw new SqlQueryException($"Cláusula '{valor}' duplicada", 0);

                        if (select_query.columns_count > 0 && select_columnPos == -1) throw new SqlQueryException($"Era esperado o nome da próxima coluna ao invés da cláusula '{valor}'", 0);
                        if (select_query.columns_count == 0) throw new SqlQueryException($"Era esperado o nome da coluna ao invés da cláusula '{valor}'", 0);

                        logic_pos++;
                        continue;
                    }

                    // 1. Seleciona a(s) coluna(s)
                    if (logic_pos == 2)
                    {
                        //if (nivelParentese > 0) throw new Exception($"'{valor_anterior}' não esperado após '{valor_anterior2}'");

                        if (select_columnPos == 1 && valor == "AS")
                        {
                            if (valor_proximo == valor) throw new SqlQueryException($"Palavra-chave Alias da coluna '{select_query.columns[select_query.columns_count - 1].column_name}' duplicado", 0);
                            continue;
                        }

                        if (valor == ",")
                        {
                            column_method = false;
                            column_methods = null;
                            column_methods_count = 0;
                            select_columnPos = -1;
                            continue; // Próxima coluna
                        }

                        if (valor_proximo == "(") // Método
                        {
                            column_method = true;
                            if (column_methods == null) column_methods = new select_column_query_method[255];
                            column_methods[column_methods_count].name = valor;
                            column_methods_count++;
                            continue;
                        }

                        if (select_columnPos == -1) select_columnPos = 0;

                        switch (select_columnPos)
                        {
                            case 0: // Coluna e métodos da coluna
                                select_query.columns[select_query.columns_count++].column_name = valor;
                                if (column_method)
                                {
                                    select_query.columns[select_query.columns_count - 1].methods = column_methods;
                                    select_query.columns[select_query.columns_count - 1].methods_count = column_methods_count;
                                }

                                nivel_colunas_select = (nivelParentese - column_methods_count); // Obtém nível principal de colunas desconsiderando os níveis usados pelos parênteses dos métodos

                                if (nivel_colunas_select > -1) // Alguma coluna está fora do nível?
                                {
                                    // Saiu do nível original do parêntese?
                                    throw new SqlQueryException($"Sintaxe incorreta próxima a '{valor}'", 0);
                                }
                                break;
                            case 1: // Nome de vizualização da Coluna
                                select_query.columns[select_query.columns_count - 1].view_column_name = valor;
                                break;
                            default:
                                throw new SqlQueryException($"'{valor_anterior}' não esperado ", 0);
                        }
                        select_columnPos++;
                        continue;
                    }

                    if (logic_pos == 1)
                    {
                        if (valor == "DISTINCT")
                        {
                            if (valor == valor_proximo) throw new SqlQueryException($"Cláusula '{valor}' duplicada", 0);
                            select_query.distinct = true;
                            logic_pos++;
                            continue;
                        }
                        else
                        {
                            // O uso do DISTINCT é opcional
                            logic_pos = 2; // Pula para a lógica 2
                            goto Inicio_Select;
                        }
                    }
                }
                #endregion
            }

            if (select && logic_pos == 1 && select_columnPos == -1) throw new SqlQueryException("Era esperado o nome da coluna", 0);
            if (select && logic_pos == 1 && (select_columnPos == 1 || select_columnPos == 2)) throw new SqlQueryException("Era esperado a cláusula 'FROM'", 0);
            if (select && logic_pos == 2 && select_query.table_name == null) throw new SqlQueryException("Era esperado o nome da tabela", 0);
            if (select && logic_pos == 500) throw new SqlQueryException("Era esperado o valor do limite", 0);
            if (select && logic_pos == 502) throw new SqlQueryException("Era esperado o valor do limite", 0);

            if (executeSelect)
            {
                Select(select_query);
            }

            if (i < query.Length) // Vai para o próximo comando sql
            {
                i++;
                goto ProximoComandoSql;
            }

            long tempoGasto = Environment.TickCount - tick;

            return null;
        }

        private void Select(select_query query)
        {
            Cab_Tabela tabela = null;

            if (!File.Exists(query.table_name))
                throw new SqlQueryException($"A tabela '{query.table_name}' não existe", 0);

            using (FileStream fs = new FileStream(query.table_name, FileMode.Open))
            using (BinaryReader br = new BinaryReader(fs))
            {
                long count = br.BaseStream.Length;
                byte[] buffer = br.ReadBytes((int)count);
                tabela = ZeroFormatterSerializer.Deserialize<Cab_Tabela>(buffer);
            }
            query.table_cab = tabela;

            //int consulta = 10000000;
            //byte[] valor_consulta = BitConverter.GetBytes(consulta);

            #region Obtém o índice das colunas
            byte[] select_colunas = new byte[query.columns_count];
            for (int i = 0; i < query.columns_count; i++)
                for (byte t = 0; t < tabela.columns.Length; t++)
                {
                    if (query.columns[i].column_name == tabela.columns[t].columnName)
                    {
                        select_colunas[i] = t;
                        break;
                    }

                    if (t == tabela.columns.Length - 1)
                        throw new SqlQueryException($"A tabela '{tabela.tableName}' não possui a coluna '{query.columns[i].column_name}'", 0);
                }
            #endregion

            long encontrados = 0;
            byte[] resultado = Select_Sequencial_Colunas_Comprimento_Fixo(
                out encontrados,
                query, select_colunas,
                query.limit.pular, query.limit.limite,
                -1, false, op.Nenhum, null);
        }

        private bool isValorString(string valor)
        {
            return (valor[0] == '\"' || valor[0] == '\'') && (valor[valor.Length - 1] == '\"' || valor[valor.Length - 1] == '\'');
        }

        private void btnSolicitar_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            txtPosfixa.Text = "";
            Update();

            dgResultado.DataSource = null;
            RPN expSql = new RPN();

            int tick = Environment.TickCount;
            //try
            //{
            //Solicitar(txtSolicitar.Text);
            char[] posfixa = null;
            Queue<char[]> strings = null;

            for (int i = 0; i < 1; i++)
            {
                posfixa = expSql.InfixaParaPosfixa(txtSolicitar.Text.ToCharArray(), out strings);
            }
            txtPosfixa.Text = new string(posfixa);
            Update();

            variavel[] variaveis = null;
            object ret = 0D;
            unsafe
            {
                fixed (char* chArray = posfixa)
                {
                    for (int i = 0; i < 1; i++)
                    {
                        ret = expSql.CalcularRPN(posfixa, strings, variaveis, out variaveis);
                    }
                }
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("coluna", typeof(double));
            DataRow r = dt.NewRow();
            r["coluna"] = ret;
            dt.Rows.Add(r);

            dgResultado.DataSource = dt;

            int tempoGasto = Environment.TickCount - tick;
                lblStatus.Text = $"Consulta executada com sucesso em {tempoGasto.ToString("N0")} ms";
            //}
            //catch (Exception ex)
            //{
            //    int tempoGasto = Environment.TickCount - tick;
            //    this.Text = tempoGasto.ToString("N0") + "ms";
            //    lblStatus.Text = ex.Message;
            //}
        }

        Queue<Cab_Tabela> queue = new Queue<Cab_Tabela>();
        private void btnMultitFileRead_Click(object sender, EventArgs e)
        {
            queue.Clear();

            ThreadPool.UnsafeQueueUserWorkItem(search, null);
            ThreadPool.UnsafeQueueUserWorkItem(search, null);
            ThreadPool.UnsafeQueueUserWorkItem(search, null);
            ThreadPool.UnsafeQueueUserWorkItem(search, null);
            //ThreadPool.UnsafeQueueUserWorkItem(search, null);
            //ThreadPool.UnsafeQueueUserWorkItem(search, null);
            //ThreadPool.UnsafeQueueUserWorkItem(search, null);
            //ThreadPool.UnsafeQueueUserWorkItem(search, null);
        }

        void Thread_Ler(object obj)
        {
            string nomeTabela = "tabela";

            Cab_Tabela tabela = null;

            using (FileStream fs = new FileStream(nomeTabela, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (BinaryReader br = new BinaryReader(fs))
            {
                int count = (int)br.BaseStream.Length;

                byte[] buffer = new byte[count];
                br.Read(buffer, 0, count);

                tabela = ZeroFormatterSerializer.Deserialize<Cab_Tabela>(buffer);
            }
            queue.Enqueue(tabela);
        }

        private void btnSql1_Click(object sender, EventArgs e)
        {
            txtSolicitar.Text = @"SELECT teste(count(id)) AS id, nome nome, idade FROM db.tabela AS P WHERE P.id = 34 OR P.id < 50 AND (P.id > 10 OR P.id = 11 AND (P.nome != 'guilherme moreira \'de alencar')) AND P.nome > 10 LIMIT 0,100";
        }

        private void btnSql2_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SET @a = 123;");
            sb.AppendLine("SET @b = 456;");
            sb.AppendLine();
            sb.AppendLine("SELECT @a + @b + 2;");

            txtSolicitar.Text = sb.ToString();
        }

        private void btnSql3_Click(object sender, EventArgs e)
        {
            txtSolicitar.Text = "SELECT 5 - 5 + 7 + (((1+2)*(3-4)/(5+6)*(7-8)) + (2-3) - 56);";
        }

        private void btnSql4_Click(object sender, EventArgs e)
        {
            txtSolicitar.Text = @"SELECT idade, nome, id FROM tabela LIMIT 0, 100";
        }

        private void btnSql5_Click(object sender, EventArgs e)
        {
            txtSolicitar.Text = "SELECT !(1 == 1);";
        }

        private void btnSql6_Click(object sender, EventArgs e)
        {
            txtSolicitar.Text = "SELECT Sum(((SELECT 1)+(SELECT 1)), 1,2,3,4) + 10;";
        }

        private void btnCreateTableDisco_Click(object sender, EventArgs e)
        {
            List<Cab_Coluna> colunas = new List<Cab_Coluna>();

            colunas.Add(new Cab_Coluna()
            {
                columnName = "id",
                dbType = DbType.Int32,
                size = 4,
                __stored_dataType = tipoDados.__int,
                __bytes = 4
            });
            colunas.Add(new Cab_Coluna()
            {
                columnName = "nome",
                dbType = DbType.String,
                size = 28,
                __stored_dataType = tipoDados.__string,
                __bytes = 28
            });
            colunas.Add(new Cab_Coluna()
            {
                columnName = "idade",
                dbType = DbType.Int32,
                size = 4,
                __stored_dataType = tipoDados.__int,
                __bytes = 4
            });

            CreateTableDisco("tabela", colunas.ToArray());
        }

        void CreateTableDisco(string nome, Cab_Coluna[] colunas)
        {
            bool bCriarCab = false;

            int tick = Environment.TickCount;
            Cab_Tabela tabela = new Cab_Tabela();
            tabela.tableName = nome;
            tabela.columns = colunas;
            colunas.Select((x, y) => new { col = x, idx = y }).ToList().ForEach(x => x.col.ordinal = x.idx);

            byte[] bytes = ZeroFormatterSerializer.Serialize(tabela);

            using (FileStream fs = new FileStream(nome, FileMode.Create))
            using (BinaryWriter bw = new BinaryWriter(fs))
            {
                bw.Write(bytes);
            }

            // Gerar Valores para a Tabela
            int quantRows = 55000000;
            int quantColunas = colunas.Count();
            int quantCampos = quantRows * colunas.Count();
            int tamanhoCol =
                4 + // row      4 bytes
                1 + // col      1 byte
                4 + // index    4 bytes
                4;  // count    4 bytes

            long tamanho_cab_dados = tamanhoCol * quantCampos;
            ulong tamanho_dados = (ulong)colunas.Sum(x => x.__bytes) * (ulong)quantRows;
            int quant_regs_por_bloco = 100000;

            int tamanho_valores_row = colunas.Sum(x => x.__bytes);

            byte[] cab_dados = null;
            if (bCriarCab) cab_dados = new byte[tamanho_cab_dados];

            #region Gera os valores da tabela
            string c_nome = "guilherme moreira de alencar";
            int c_idade = 30;

            int tickGerarDados = Environment.TickCount;
            ParallelOptions opt = new ParallelOptions();
            opt.MaxDegreeOfParallelism = 1;

            string path_cab_dados = nome + "_cab_dados.bin";
            string path_dados = nome + "_dados.bin";

            using (FileStream fs = new FileStream(path_dados, FileMode.Create))
            using (BinaryWriter bw = new BinaryWriter(fs))
            {
                byte[] dados_bloco = new byte[colunas.Sum(x => x.__bytes) * quant_regs_por_bloco];
                int posByteDados = 0;

                int posByteCampo = -1;
                int row = -1;
                byte col = 0;
                byte[] bytes_valor = null;
                int id = -1;

                for (int campoAtual = 0; campoAtual < quantCampos; campoAtual++) // Gera 'cab_valores.bin'
                {
                    posByteCampo = (campoAtual * tamanhoCol);
                    row = (campoAtual / quantColunas);
                    col = (byte)(campoAtual - (quantColunas * row));

                    #region Cria o valor
                    id = row;

                    if (col == 0) bytes_valor = BitConverter.GetBytes(id);
                    else if (col == 1) bytes_valor = ASCIIEncoding.ASCII.GetBytes(c_nome);
                    else if (col == 2) bytes_valor = BitConverter.GetBytes(c_idade);
                    int valor_count = colunas[col].__bytes; // Coluna de tamanho fixo

                    if (posByteDados == dados_bloco.Length)
                    {
                        bw.Write(dados_bloco);
                        posByteDados = 0;
                    }

                    Buffer.BlockCopy(bytes_valor, 0, dados_bloco, posByteDados, bytes_valor.Length);
                    posByteDados += colunas[col].__bytes; // Obtém posição pela soma dos bytes

                    // Index e count para o cabeçalho do valor
                    int index = posByteDados;
                    int count = valor_count;
                    #endregion

                    #region Cria o cabeçalho do valor
                    if (bCriarCab)
                    {
                        byte[] bytesRow = BitConverter.GetBytes(row);
                        byte[] bytesIndex = BitConverter.GetBytes(index);
                        byte[] bytesCount = BitConverter.GetBytes(count);

                        int posByte = posByteCampo;

                        Buffer.BlockCopy(bytesRow, 0, cab_dados, posByte, bytesRow.Length);
                        posByte += bytesRow.Length;
                        cab_dados[posByte++] = col;
                        Buffer.BlockCopy(bytesIndex, 0, cab_dados, posByte, bytesIndex.Length);
                        posByte += bytesIndex.Length;
                        Buffer.BlockCopy(bytesCount, 0, cab_dados, posByte, bytesCount.Length);
                        posByte += bytesCount.Length;
                    }
                    #endregion
                }
                dados_bloco = null;
                cab_dados = null;
                
                GC.Collect();
            }

            int tempoGastoGerarDados = Environment.TickCount - tickGerarDados;
            #endregion

            if (bCriarCab)
            {
                using (FileStream fs = new FileStream(path_cab_dados, FileMode.Create))
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    bw.Write(cab_dados);
                }
                cab_dados = null;
                GC.Collect();
            }

            int tempoGasto = Environment.TickCount - tick;
            MessageBox.Show(this, $"Tabela '{nome}' criada com {quantRows.ToString("N0")} registros gerados em {tempoGasto.ToString("N0")}ms");
        }

        private void btnSql7_Click(object sender, EventArgs e)
        {
            txtSolicitar.Text = @"select 5 + ((7 / 6 + 7 + (sin(2*2-8 - cos(2) - 5 * 3 + 6 /4) * 3 + 6 - tan(34)) * 2 + 5 - 6) - 5) + 3 * 2";
        }
    }
}