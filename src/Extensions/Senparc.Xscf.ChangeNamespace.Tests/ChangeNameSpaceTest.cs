using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Senparc.Xscf.ChangeNamespace.Functions;

namespace Senparc.Xscf.ChangeNamespace.Tests
{
    [TestClass]
    public class ChangeNameSpaceTest
    {
        [TestMethod]
        public void RunTest()
        {
            var services = new ServiceCollection();
            var function = new ChangeNameSpace(services);
            var path = @"E:\SenparcÏîÄ¿\SenparcCoreFramework\ScfPackageSources\src\Extensions\Senparc.Xscf.ChangeNamespace.Tests\App_Data\src";
            var newNameSpace = "This.Is.NewNamespace.";
            function.Run(path, newNameSpace);

        }
    }
}
