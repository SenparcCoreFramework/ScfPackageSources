using Microsoft.VisualStudio.TestTools.UnitTesting;
using Senparc.Scf.Core.Enums;
using Senparc.Scf.Core.Tests;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Senparc.Scf.XscfBase.Tests
{
    public class TestModule : XscfRegisterBase, IXscfRegister
    {
        public override string Name => "Senparc.Scf.XscfBase.Tests.TestModule";

        public override string Uid => "000111";

        public override string Version => "1.0";

        public override string MenuName => "测试模块";
        public override string Icon => "fa fa-space-shuttle";//参考如：https://colorlib.com/polygon/gentelella/icons.html
       
        public override string Description => "这是测试模块的介绍";

        public override IList<Type> Functions => new List<Type>() { typeof(FunctionBaseTest_Function) };

        public override Task InstallOrUpdateAsync(InstallOrUpdate installOrUpdate)
        {
            Console.WriteLine(installOrUpdate);
            return Task.CompletedTask;
        }

        public override async Task UninstallAsync(Func<Task> unsinstallFunc)
        {
            Console.WriteLine("Uninstall");
            await unsinstallFunc().ConfigureAwait(false);
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
