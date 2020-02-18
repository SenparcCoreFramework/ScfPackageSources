using Microsoft.VisualStudio.TestTools.UnitTesting;
using Senparc.Scf.Core.Tests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Scf.XscfBase.Tests
{
    public class TestModule : IXscfRegister
    {
        public string Name => "Senparc.Scf.XscfBase.Tests.TestModule";

        public string Uid => "000111";

        public string Version => "1.0";

        public string MenuName => "测试模块";

        public string Description => "这是测试模块的介绍";

        public IList<Type> Functions => new List<Type>() { typeof(FunctionBaseTest_Function) };

        public void Install()
        {
            Console.WriteLine("Install");
        }

        public void Uninstall()
        {
            Console.WriteLine("Uninstall");
        }
    }

    [TestClass]
    public class RegisterTest : TestBase
    {
        [TestMethod]
        public void StartEngineTest()
        {
            var result = base.ServiceCollection.StartEngine();
            Console.WriteLine(result);
            Assert.IsTrue(Senparc.Scf.XscfBase.Register.RegisterList.Count > 0);
        }
    }
}
