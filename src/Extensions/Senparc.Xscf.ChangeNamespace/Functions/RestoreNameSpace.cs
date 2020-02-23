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
        [Description("当前自定义的命名空间||命名空间根，一般以.结尾，如：[My.Namespace.]，最终将替换为[Senparc.Scf.]")]
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
            var typeParam = param as ChangeNamespace_Parameters;

            StringBuilder sb = new StringBuilder();
            base.RecordLog(sb, "开始运行 RestoreNameSpace");

            var path = typeParam.Path;
            var myNamespace = typeParam.NewNamespace;

            base.RecordLog(sb, $"path:{path} myNamespace:{myNamespace}");

            var meetRules = new List<MeetRule>() {
                new MeetRule($"namespace {myNamespace}","namespace Senparc.Scf.","*.cs"),
                new MeetRule($"namespace {myNamespace}","namespace Senparc.","*.cs"),
                new MeetRule($"@model {myNamespace}","@model Senparc.Scf.","*.cshtml"),
                new MeetRule($"@model {myNamespace}","@model Senparc.","*.cshtml"),
            };

            foreach (var item in meetRules)
            {
                var files = Directory.GetFiles(path, item.FileType, SearchOption.AllDirectories);
                base.RecordLog(sb, $"文件类型:{item.FileType} 数量:{files.Length}");

                foreach (var file in files)
                {
                    string content = null;
                    using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                    {
                        using (var sr = new StreamReader(fs))
                        {
                            content = sr.ReadToEnd();
                        }
                        fs.Close();
                    }

                    if (content.IndexOf(item.OrignalKeyword) >= 0)
                    {
                        base.RecordLog(sb, $"文件命中:{file}");

                        content = content.Replace(item.OrignalKeyword, item.ReplaceWord);
                        using (var fs = new FileStream(file, FileMode.Truncate, FileAccess.ReadWrite))
                        {
                            using (var sw = new StreamWriter(fs))
                            {
                                sw.Write(content);
                                sw.Flush();
                            }
                            fs.Close();
                        }
                    }
                }

            }

            return sb.ToString();
        }

    }
}
