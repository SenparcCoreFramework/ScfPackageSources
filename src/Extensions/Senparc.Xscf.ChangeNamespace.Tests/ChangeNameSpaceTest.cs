using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Senparc.Xscf.ChangeNamespace.Functions;
using System;
using System.IO;

namespace Senparc.Xscf.ChangeNamespace.Tests
{
    [TestClass]
    public class ChangeNameSpaceTest
    {
        [TestMethod]
        public void RunTest()
        {
            var serviceProvider = new ServiceCollection().BuildServiceProvider();
            var function = new Functions.ChangeNamespace(serviceProvider);

            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\", @"App_Data\src");
            var newNameSpace = "This.Is.NewNamespace.";

            //TODO:≤‚ ‘∑¥œÚ ‰»Î

            var result = function.Run(new ChangeNamespace_Parameters()
            {
                Path = path,
                NewNamespace = newNameSpace
            });

            Assert.AreEqual(function.FunctionParameterType, typeof(ChangeNamespace_Parameters));

            System.Console.WriteLine(result);
        }
    }
}
