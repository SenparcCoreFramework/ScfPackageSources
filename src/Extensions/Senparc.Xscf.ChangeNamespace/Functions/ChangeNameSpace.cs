using Senparc.CO2NET.Extensions;
using Senparc.Scf.XscfBase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;

namespace Senparc.Xscf.ChangeNamespace.Functions
{

    public class MatchNamespace
    {
        public string Prefix { get; set; }
        public string OldNamespace { get; set; }
        public string NewNamespace { get; set; }
    }
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

    public class ChangeNamespace : FunctionBase
    {
        //注意：Name 必须在单个 Xscf 模块中唯一！
        public override string Name => "修改命名空间";

        public override string Description => "修改所有源码在 .cs, .cshtml 中的命名空间";

        public override Type FunctionParameterType => typeof(ChangeNamespace_Parameters);

        public ChangeNamespace(IServiceProvider serviceProvider) : base(serviceProvider)
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
            base.RecordLog(sb, "开始运行 ChangeNamespace");

            var path = typeParam.Path;
            var newNamespace = typeParam.NewNamespace;

            base.RecordLog(sb, $"path:{path} newNamespace:{newNamespace}");

            var meetRules = new List<MeetRule>() {
                //new MeetRule("namespace Senparc.Scf.",$"namespace {newNamespace}","*.cs"),
                new MeetRule("namespace","Senparc.",$"{newNamespace}","*.cs"),
                //new MeetRule("@model Senparc.Scf.",$"@model {newNamespace}","*.cshtml"),
                new MeetRule("@model","Senparc.",$"{newNamespace}","*.cshtml"),
            };

            //TODO:使用正则记录，并全局修改

            Dictionary<string, List<MatchNamespace>> namespaceCollection = new Dictionary<string, List<MatchNamespace>>(StringComparer.OrdinalIgnoreCase);


            foreach (var item in meetRules)
            {
                var files = Directory.GetFiles(path, item.FileType, SearchOption.AllDirectories);

                //扫描所有 namespace
                foreach (var file in files)
                {

                    base.RecordLog(sb, $"扫描文件类型:{item.FileType} 数量:{files.Length}");

                    //string content = null;
                    using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                    {
                        using (var sr = new StreamReader(fs))
                        {
                            var line = sr.ReadLine();
                            while (null != line)
                            {
                                line = sr.ReadLine()?.Trim();
                                var oldNamespaceFull = $"{item.Prefix} {item.OrignalKeyword}";
                                if (line != null && line.StartsWith(oldNamespaceFull))
                                {
                                    if (!namespaceCollection.ContainsKey(file))
                                    {
                                        namespaceCollection[file] = new List<MatchNamespace>();
                                    }
                                    var oldNamespaceArr = line.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                                    var getOld = oldNamespaceArr[1];
                                    var getNew = oldNamespaceArr[1].Replace(item.OrignalKeyword, item.ReplaceWord);
                                    namespaceCollection[file].Add(new MatchNamespace()
                                    {
                                        Prefix = oldNamespaceArr[0],//prefix
                                        OldNamespace = getOld,
                                        NewNamespace = getNew
                                    });

                                    namespaceCollection[file].Add(new MatchNamespace()
                                    {
                                        Prefix = "using",
                                        OldNamespace = getOld,
                                        NewNamespace = getNew
                                    });
                                }

                                //content += Environment.NewLine + line;
                            }
                            sr.ReadLine();
                        }
                        fs.Close();
                    }
                }

                //替换
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

                    foreach (var namespaceInfos in namespaceCollection)
                    {
                        foreach (var namespaceInfo in namespaceInfos.Value)
                        {
                            var oldNamespaceFull = $"{namespaceInfo.Prefix} {namespaceInfo.OldNamespace}";

                            //替换旧的NameSpace
                            if (content.IndexOf(oldNamespaceFull) > -1)
                            {
                                base.RecordLog(sb, $"文件命中:{file} -> {oldNamespaceFull}");
                                var newNameSpaceFull = $"{namespaceInfo.Prefix} {namespaceInfo.NewNamespace}";
                                content = content.Replace(oldNamespaceFull, newNameSpaceFull);
                            }
                        }
                    }

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

            return sb.ToString();
        }

    }
}
