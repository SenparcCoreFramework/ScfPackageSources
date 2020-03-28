using Microsoft.EntityFrameworkCore;
using Senparc.Scf.Core.Models;
using Senparc.Scf.Service;
using Senparc.Scf.XscfBase;
using Senparc.Scf.XscfBase.Functions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Senparc.Xscf.DatabaseToolkit.Functions
{
    public class BackupDatabase : FunctionBase
    {
        public class BackupDatabase_Parameters : FunctionParameterLoadDataBase
        {
            [Required]
            [MaxLength(300)]
            [Description("路径||本地物理路径，如：E:\\Senparc\\Database-Backup\\SCF.bak，必须包含文件名。请确保此路径有网站程序访问权限！")]
            public string Path { get; set; }

            public override async Task LoadData(IServiceProvider serviceProvider)
            {
                var configService = serviceProvider.GetService<ServiceBase<DbConfig>>();
                var config = await configService.GetObjectAsync(z => true);
                if (config != null)
                {
                    Path = config.BackupPath;
                }
            }
        }

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
            return FunctionHelper.RunFunction<BackupDatabase_Parameters>(param, (typeParam, sb, result) =>
            {
                try
                {
                    var path = typeParam.Path;
                    if (File.Exists(path))
                    {
                        var copyPath = path + ".last.bak";
                        RecordLog(sb, "检测到同名文件，已经移动到（并覆盖）：" + copyPath);
                        File.Move(path, copyPath, true);
                    }

                    RecordLog(sb, "开始获取 ISenparcEntities 对象");
                    var senparcEntities = ServiceProvider.GetService(typeof(ISenparcEntities)) as SenparcEntitiesBase;
                    RecordLog(sb, "获取 ISenparcEntities 对象成功");
                    var sql = $@"Backup Database {senparcEntities.Database.GetDbConnection().Database} To disk='{path}'";
                    RecordLog(sb, "准备执行 SQL：" + sql);
                    int affectRows = senparcEntities.Database.ExecuteSqlRaw(sql);
                    RecordLog(sb, "执行完毕，备份结束。affectRows：" + affectRows);

                    RecordLog(sb, "检查备份文件：" + path);
                    if (File.Exists(path))
                    {
                        var modifyTime = File.GetLastWriteTimeUtc(path);
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
                    result.Message += ex.Message;
                    throw;
                }

            });
        }
    }
}
