using Senparc.Scf.XscfBase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Senparc.Xscf.ChangeNamespace.Functions
{
    public class ChangeNamespace : FunctionBase
    {
        public override IList<FunctionParam> FunctionParams
            => new List<FunctionParam>() {
                new FunctionParam("路径","本地物理路径，如：E:\\Senparc\\Scf\\", TypeCode.String),
                new FunctionParam("新命名空间","命名空间根，必须以.结尾，用于替换[Senparc.Scf.]", TypeCode.String)
                };

        public ChangeNamespace(IServiceProvider serviceProvider):base(serviceProvider)
        {
        }

        /// <summary>
        /// <para>参数1：path</para>
        /// <para>参数2：newNamespace，以.结尾</para>
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override string Run(params object[] param)
        {
            StringBuilder sb = new StringBuilder();
            base.RecordLog(sb, "开始运行 ChangeNamespace");

            var path = param[0] as string;
            var newNamespace = param[1] as string;

            base.RecordLog(sb, $"path:{path} newNamespace:{newNamespace}");

            var meetRules = new List<MeetRule>() {
                new MeetRule("namespace Senparc.Scf.",$"namespace {newNamespace}","*.cs"),
                new MeetRule("namespace Senparc.",$"namespace {newNamespace}","*.cs"),
                new MeetRule("@model Senparc.Scf.",$"@model {newNamespace}","*.cshtml"),
                new MeetRule("@model Senparc.",$"@model {newNamespace}","*.cshtml"),
            };

            foreach (var item in meetRules)
            {
                var files = Directory.GetFiles(path, item.FileType);
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
                        using (var fs = new FileStream(file, FileMode.Truncate, FileAccess.Read))
                        {
                            using (var sw = new StreamWriter(fs))
                            {
                                sw.Write(content);
                            }
                            fs.Flush();
                            fs.Close();
                        }
                    }
                }

            }

            return sb.ToString();//TODO:统一变成日志记录
        }
    }
}
