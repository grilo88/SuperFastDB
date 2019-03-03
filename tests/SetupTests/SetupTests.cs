using System;
using System.Configuration;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperFastDB;

namespace SetupTests
{
    [TestClass]
    public class SetupTests
    {
        [TestMethod]
        public void InstalarServico()
        {
            Setup setup = new Setup();
            setup.InstalarServico(@"..\..\..\..\src\SuperFastDB_Server\bin\debug\SuperFastDB_Server.exe");
        }

        [TestMethod]
        public void DesinstalarServico()
        {
            Setup setup = new Setup();
            setup.DesinstalarServico(@"..\..\..\..\src\SuperFastDB_Server\bin\debug\SuperFastDB_Server.exe");
        }
    }
}