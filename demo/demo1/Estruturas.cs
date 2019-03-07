using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroFormatter;

namespace SuperFast
{
    public enum tipoDados : byte
    {
        __binary,
        __string,
        __char,
        __sbyte,
        __short,
        __int24,
        __int,
        __long,
        __byte,
        __ushort,
        __uint24,
        __ulong,
        __float,
        __double,
        __uint,
        __decimal
    }

    public enum op : byte
    {
        Nenhum,
        IgualA,
        DiferenteDe,
        MaiorQue,
        MaiorOuIgualA,
        MenorQue,
        MenorOuIgualA,
        IniciaCom,
        TerminaCom,
        Contem,
    }

    public struct Header
    {
        public int type;
        public int row;
        public int col;
        public int index;
        public int count;
    }

    public enum tipoPalavraChave
    {
        Select,
        Where,
        Insert,
        Update,
        Delete
    }

    public class SqlValor
    {
        public Cab_Coluna coluna { get; set; }
        public string valor { get; set; }
    }

    public class SqlPalavraChave
    {
        public tipoPalavraChave palavraChave { get; set; }
        public SqlValor valor { get; set; }

    }

    public class SqlQueryExp
    {
        public string exp { get; set; }
        public bool result { get; set; }
        public SqlQueryExp inner { get; set; }
    }

    [ZeroFormattable]
    public class Cab_Tabela
    {
        [Index(0)]
        public virtual string tableName { get; set; }
        [Index(1)]
        public virtual Cab_Coluna[] columns { get; set; }
    }

    [ZeroFormattable]
    public class Cab_Coluna
    {
        [Index(0)]
        public virtual string columnName { get; set; }
        [Index(1)]
        public virtual DbType dbType { get; set; }
        [Index(2)]
        public virtual int size { get; set; }
        [Index(3)]
        public virtual int ordinal { get; set; }
        [Index(4)]
        public virtual int __bytes { get; set; }
        [Index(5)]
        public virtual tipoDados __stored_dataType { get; set; }
    }

    public struct Cab_Dados
    {
        public int row;
        public byte col;
        public int index;
        public int count;
    }

}
