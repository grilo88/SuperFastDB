using Microsoft.VisualStudio.TestTools.UnitTesting;
using InterpretadorSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterpretadorSQL.Tests
{
    [TestClass()]
    public class RPNTests
    {
        RPN rpn = new RPN();

        [TestMethod()]
        public void RPNTest()
        {
        }

        [TestMethod()]
        public void InfixaParaPosfixaTest()
        {
            // Teste inicial
            string saida = InfixaParaPosFixa("Select 1");
            Assert.AreEqual(saida, "1 SL");

            // Teste inicial
            //saida = InfixaParaPosFixa("Select --1");
            //Assert.AreEqual(saida, "1 SL");

            // Separação regular de operandos e operados 
            saida = InfixaParaPosFixa("select 5 + ((7 / 6 + 7 + (sin(2 * 2 - 8 - cos(2) - 5 * 3 + 6 / 4) * 3 + 6 - tan(34)) * 2 + 5 - 6) - 5) + 3 * 2");
            Assert.AreEqual(saida, "5 7 6 / 7 + 2 2 * 8 - 2 CO - 5 3 * - 6 4 / + SI 3 * 6 + 34 TG - 2 * + 5 + 6 - 5 - + 3 2 * + SL");

            // Operandos e operadores juntos
            saida = InfixaParaPosFixa("select 5+((7/6+7+(sin(2*2-8-cos(2)-5*3+6/4)*3+6-tan(34))*2+5-6)-5)+3*2");
            Assert.AreEqual(saida, "5 7 6 / 7 + 2 2 * 8 - 2 CO - 5 3 * - 6 4 / + SI 3 * 6 + 34 TG - 2 * + 5 + 6 - 5 - + 3 2 * + SL");

            // Espaçamentos irregulares entre operandos, operadores e funções
            saida = InfixaParaPosFixa("select 5+(  (7/   6+   7+(   sin   (2*  2-   8   -   cos( 2)- 5* 3+   6/  4)*3+6-tan   (  34))*2    +5-6   )-  5)+3   *2");
            Assert.AreEqual(saida, "5 7 6 / 7 + 2 2 * 8 - 2 CO - 5 3 * - 6 4 / + SI 3 * 6 + 34 TG - 2 * + 5 + 6 - 5 - + 3 2 * + SL");
        }

        private string InfixaParaPosFixa(string sql)
        {
            char[] infixa = sql.ToCharArray();
            char[] infixa_u = sql.ToUpper().ToCharArray();

            Queue<char[]> out_strings;
            char[] ret = rpn.InfixaParaPosfixa(infixa, infixa_u, out out_strings);

            return new string(ret, 0, rpn.ComprimentoRPN);
        }

        [TestMethod()]
        public void AvaliarTest()
        {
        }

        [TestMethod()]
        public void CustomToDecimalTest()
        {
            char[] num = "10".ToCharArray();

            decimal saida = rpn.CustomToDecimal(num, 0, num.Length - 1, false);
            Assert.AreEqual(saida, (decimal)10);

            saida = rpn.CustomToDecimal(num, 0, num.Length - 1, true);
            Assert.AreEqual(saida, (decimal)-10);

            num = "0.564345".ToCharArray();
            saida = rpn.CustomToDecimal(num, 0, num.Length - 1, false);
            Assert.AreEqual(saida, (decimal)0.564345);

            num = "0.564345".ToCharArray();
            saida = rpn.CustomToDecimal(num, 0, num.Length - 1, true);
            Assert.AreEqual(saida, (decimal)-0.564345);
        }

        [TestMethod()]
        public void isDigitoTest()
        {
        }

        [TestMethod()]
        public void isLetraTest()
        {
        }

        [TestMethod()]
        public void isCaractereVarTest()
        {
        }

        [TestMethod()]
        public void isNumericoTest()
        {
        }

        [TestMethod()]
        public void GarantirCapacidadeTest()
        {
        }
    }
}