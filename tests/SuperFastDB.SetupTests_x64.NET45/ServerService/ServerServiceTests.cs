using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SuperFastDB.Setup_x64.NET45
{
    [TestClass]
    public class ServerServiceTests
    {
        [TestMethod]
        public void _001_InstallServerService()
        {
            SetupTests.ServerServiceTests.InstallServerService();
        }

        [TestMethod]
        public void _002_UninstallServerService()
        {
            SetupTests.ServerServiceTests.UninstallServerService();
        }
    }
}
