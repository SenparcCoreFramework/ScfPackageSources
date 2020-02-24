using Senparc.Scf.XscfBase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;

namespace Senparc.Xscf.ChangeNamespace.Functions
{
    public class RestoreNameSpace_Parameters : IFunctionParameter
    {
        [Required]
        [MaxLength(300)]
        [Description("路径||本地物理路径，如：E:\\Senparc\\Scf\\")]
        public string Path { get; set; }
        [Required]
        [MaxLength(100)]
        [Description("当前自定义的命名空间||命名空间根，一般以.结尾，如：[My.Namespace.]，最终将替换为例如[Senparc.Scf.]或[Senparc.]")]
        public string MyNamespace { get; set; }
    }

    /// <summary>
    /// 还原命名空间
    /// </summary>
    public class RestoreNameSpace : FunctionBase
    {
        //注意：Name 必须在单个 Xscf 模块中唯一！
        public override string Name => "还原命名空间";

        public override string Description => "还原所有源码在 .cs, .cshtml 中的命名空间为 SCF 默认（建议在断崖式更新之前进行此操作）";

        public override Type FunctionParameterType => typeof(RestoreNameSpace_Parameters);

        public RestoreNameSpace(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        /// <summary>
        /// 运行
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override string Run(IFunctionParameter param)
        {
            var typeParam = param as RestoreNameSpace_Parameters;
            var changeNamespaceParam = new ChangeNamespace_Parameters()
            {
                NewNamespace = "Senparc.",
                Path = typeParam.Path
            };
            ChangeNamespace changeNamespaceFunction = new ChangeNamespace(base.ServiceProvider);
            changeNamespaceFunction.OldNamespaceKeyword = typeParam.MyNamespace;
            var result = changeNamespaceFunction.Run(changeNamespaceParam);
            return result;
        }
    }
}
