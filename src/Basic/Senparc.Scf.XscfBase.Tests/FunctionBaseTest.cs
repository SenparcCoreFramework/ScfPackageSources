using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Senparc.Scf.XscfBase.Tests
{
    public class FunctionBaseTest_Function : FunctionBase<FunctionBaseTest_FunctionParameter>
    {
        public FunctionBaseTest_Function(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override string Name => "测试方法";

        public override string Description => "测试方法说明";

        public override string Run(FunctionBaseTest_FunctionParameter param)
        {
            Console.WriteLine("Run");
            return "OK";
        }
    }

    public class FunctionBaseTest_FunctionParameter : IFunctionParameter
    {
        [Required]
        [MaxLength(300)]
        [System.ComponentModel.Description("路径||本地物理路径，如：E:\\Senparc\\Scf\\")]
        public string Path { get; set; }

        [MaxLength(100)]
        [System.ComponentModel.Description("新命名空间||命名空间根，必须以.结尾，用于替换[Senparc.Scf.]")]
        public string NewNamespace { get; set; }
    }

    [TestClass]
    public class FunctionBaseTest
    {
        [TestMethod]
        public void GetFunctionParammeterInfo()
        {
            FunctionBaseTest_Function function = new FunctionBaseTest_Function(null);
            var paraInfo = function.GetFunctionParammeterInfo().ToList();

            Assert.AreEqual(2, paraInfo.Count);

            Assert.AreEqual("Path", paraInfo[0].Name);
            Assert.AreEqual("路径", paraInfo[0].Title);
            Assert.AreEqual("本地物理路径，如：E:\\Senparc\\Scf\\", paraInfo[0].Description);
            Assert.AreEqual(true, paraInfo[0].IsRequired);

            Assert.AreEqual("NewNamespace", paraInfo[1].Name);
            Assert.AreEqual("新命名空间", paraInfo[1].Title);
            Assert.AreEqual("命名空间根，必须以.结尾，用于替换[Senparc.Scf.]", paraInfo[1].Description);
        }
    }
}
