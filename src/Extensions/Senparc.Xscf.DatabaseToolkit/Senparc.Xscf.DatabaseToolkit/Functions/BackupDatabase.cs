using Microsoft.EntityFrameworkCore;
using Senparc.Scf.Core.Models;
using Senparc.Scf.XscfBase;
using Senparc.Scf.XscfBase.Functions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;

namespace Senparc.Xscf.DatabaseToolkit.Functions
{
    public class BackupDatabase : FunctionBase
    {
        //注意：Name 必须在单个 Xscf 模块中唯一！
        public override string Name => "备份数据库";

        public override string Description => "将当前使用的数据库备份到指定路径。友情提示：建议确保该路径不具备公开访问权限！";

        public override Type FunctionParameterType => typeof(BackupDatabase_Parameters);

        public BackupDatabase(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        /// <summary>
        /// 运行
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override FunctionResult Run(IFunctionParameter param)
        {
            /* 这里是处理文字选项（单选）的一个示例 */
            var typeParam = param as BackupDatabase_Parameters;
            StringBuilder sb = new StringBuilder();
            FunctionResult result = new FunctionResult()
            {
                Success = true
            };

            try
            {
                RecordLog(sb, "开始获取 ISenparcEntities 对象");
                var senparcEntities = ServiceProvider.GetService(typeof(ISenparcEntities)) as SenparcEntitiesBase;
                RecordLog(sb, "获取 ISenparcEntities 对象成功");
                var sql = $@"Backup Database {senparcEntities.Database.GetDbConnection().Database} To disk='{typeParam.Path}'";
                RecordLog(sb, "准备执行 SQL：" + sql);
                int affectRows = senparcEntities.Database.ExecuteSqlRaw(sql);
                RecordLog(sb, "执行完毕，备份结束。affectRows：" + affectRows);

                RecordLog(sb, "检查备份文件：" + typeParam.Path);
                if (File.Exists(typeParam.Path))
                {
                    var modifyTime = File.GetLastWriteTimeUtc(typeParam.Path);
                    if ((SystemTime.UtcDateTime - modifyTime).TotalSeconds < 5/*5秒钟内创建的*/)
                    {
                        RecordLog(sb, "检查通过，备份成功！最后修改时间：" + modifyTime.ToString());
                        result.Message = "备份完成！";
                    }
                    else
                    {
                        result.Message = $"文件存在，但修改时间不符，可能未备份成功，请检查文件！文件最后修改时间：{modifyTime.ToString()}";
                        RecordLog(sb, result.Message);
                    }
                }
                else
                {
                    result.Message = "备份文件未生成，备份失败！";
                    RecordLog(sb, result.Message);

                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Exception = new XscfFunctionException(ex.Message, ex);
                result.Message = "备份失败！";

                RecordLog(sb, "发生错误：" + ex.Message);
                RecordLog(sb, ex.StackTrace.ToString());
            }

            result.Log = sb.ToString();
            return result;
        }

        public class BackupDatabase_Parameters : IFunctionParameter
        {
            [Required]
            [MaxLength(300)]
            [Description("路径||本地物理路径，如：E:\\Senparc\\Database-Backup\\SCF.bak，必须包含文件名。请确保此路径有网站程序访问权限！")]
            public string Path { get; set; }
        }
    }
}
