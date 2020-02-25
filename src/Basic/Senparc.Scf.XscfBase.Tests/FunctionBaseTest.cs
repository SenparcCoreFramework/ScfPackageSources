using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Senparc.Scf.Core.Tests;
using Senparc.Scf.XscfBase.Functions;

namespace Senparc.Scf.XscfBase.Tests
{
    public class FunctionBaseTest_Function : FunctionBase
    {
        public FunctionBaseTest_Function(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override string Name => "测试方法";

        public override string Description => "测试方法说明";

        public override Type FunctionParameterType => typeof(FunctionBaseTest_FunctionParameter);

        public override FunctionResult Run(IFunctionParameter param)
        {
            Console.WriteLine("Run");
            FunctionResult result = new FunctionResult()
            {
                Success = true,
                Message = "OK",
                Log = "This is Log"
            };
            return result;
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


        [System.ComponentModel.Description("网站||选择需要下载的网站")]
        public string[] Site { get; set; } = new[] { "", "GitHub", "Gitee" };
    }

    [TestClass]
    public class FunctionBaseTest : TestBase
    {
        [TestMethod]
        public void GetFunctionParammeterInfo()
        {
            FunctionBaseTest_Function function = new FunctionBaseTest_Function(null);
            var paraInfo = function.GetFunctionParammeterInfo().ToList();

            Assert.AreEqual(3, paraInfo.Count);

            Assert.AreEqual("Path", paraInfo[0].Name);
            Assert.AreEqual("路径", paraInfo[0].Title);
            Assert.AreEqual("本地物理路径，如：E:\\Senparc\\Scf\\", paraInfo[0].Description);
            Assert.AreEqual(true, paraInfo[0].IsRequired);
            Assert.AreEqual("String", paraInfo[0].SystemType);
            Assert.AreEqual(ParammeterType.Text, paraInfo[0].ParammeterType);

            Assert.AreEqual("NewNamespace", paraInfo[1].Name);
            Assert.AreEqual("新命名空间", paraInfo[1].Title);
            Assert.AreEqual(ParammeterType.Text, paraInfo[1].ParammeterType);
            Assert.AreEqual("命名空间根，必须以.结尾，用于替换[Senparc.Scf.]", paraInfo[1].Description);

            Assert.AreEqual(ParammeterType.SingleSelection, paraInfo[2].ParammeterType);
            Assert.AreEqual("Site", paraInfo[2].Name);
            Assert.AreEqual("网站", paraInfo[2].Title);
            Assert.AreEqual("选择需要下载的网站", paraInfo[2].Description);
            Assert.AreEqual(3, paraInfo[2].SelectionItems.Count());
            Assert.AreEqual("", paraInfo[2].SelectionItems[0]);
            Assert.AreEqual("GitHub", paraInfo[2].SelectionItems[1]);
            Assert.AreEqual("Gitee", paraInfo[2].SelectionItems[2]);

        }
    }
}
