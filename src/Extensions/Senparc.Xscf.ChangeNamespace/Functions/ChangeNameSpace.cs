using Senparc.Scf.XscfBase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;

namespace Senparc.Xscf.ChangeNamespace.Functions
{
    public class ChangeNamespace_Parameters : IFunctionParameter
    {
        [Required]
        [MaxLength(300)]
        [Description("路径||本地物理路径，如：E:\\Senparc\\Scf\\")]
        public string Path { get; set; }
        [Required]
        [MaxLength(100)]
        [Description("新命名空间||命名空间根，必须以.结尾，用于替换[Senparc.Scf.]")]
        public string NewNamespace { get; set; }
    }

    public class ChangeNamespace : FunctionBase<ChangeNamespace_Parameters>
    {
        public override string Name => "修改命名空间";

        public override string Description => "修改所有源码在 .cs, .cshtml 中的命名空间";


        public ChangeNamespace(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        /// <summary>
        /// 运行
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override string Run(ChangeNamespace_Parameters param)
        {
            StringBuilder sb = new StringBuilder();
            base.RecordLog(sb, "开始运行 ChangeNamespace");

            var path = param.Path;
            var newNamespace = param.NewNamespace;

            base.RecordLog(sb, $"path:{path} newNamespace:{newNamespace}");

            var meetRules = new List<MeetRule>() {
                new MeetRule("namespace Senparc.Scf.",$"namespace {newNamespace}","*.cs"),
                new MeetRule("namespace Senparc.",$"namespace {newNamespace}","*.cs"),
                new MeetRule("@model Senparc.Scf.",$"@model {newNamespace}","*.cshtml"),
                new MeetRule("@model Senparc.",$"@model {newNamespace}","*.cshtml"),
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
