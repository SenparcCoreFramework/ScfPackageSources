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
                var sql = $@"Backup Database {senparcEntities.Database.GetDbConnection().DataSource} To disk='{param}'";
                RecordLog(sb, "准备执行 SQL：" + sql);
                int affectRows = senparcEntities.Database.ExecuteSqlRaw(sql);
                RecordLog(sb, "执行完毕，备份结束");
                result.Message = "备份成功。为进一步确保，建议您核对备份文件修改时间。";
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
