using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Scf.XscfBase.Tests
{
    [TestClass]
    public class RegisterTest
    {
        [TestMethod]
        public void StartEngineTest()
        {
            var result = Senparc.Scf.XscfBase.Register.StartEngine();
            //Assert.IsTrue(Senparc.Scf.XscfBase.Register.RegisterList.Count > 0);
            //Console.WriteLine(result);
        }
    }
}
