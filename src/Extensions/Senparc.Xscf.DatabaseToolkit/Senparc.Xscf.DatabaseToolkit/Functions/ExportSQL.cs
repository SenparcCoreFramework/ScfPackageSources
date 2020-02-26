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
    public class ExportSQL : FunctionBase
    {
        public class ExportSQL_Parameters : IFunctionParameter
        {

        }

        //注意：Name 必须在单个 Xscf 模块中唯一！
        public override string Name => "Merge EF Core";

        public override string Description => "使用 Entity Framework Core 的 Code First 模式对数据库进行更新，使数据库和当前运行版本匹配。";

        public override Type FunctionParameterType => typeof(ExportSQL_Parameters);

        public ExportSQL(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        /// <summary>
        /// 运行
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override FunctionResult Run(IFunctionParameter param)
        {
            return FunctionHelper.RunFunction<ExportSQL_Parameters>(param, (typeParam, sb, result) =>
             {
                 RecordLog(sb, "开始获取 ISenparcEntities 对象");
                 var senparcEntities = ServiceProvider.GetService(typeof(ISenparcEntities)) as SenparcEntitiesBase;
                 RecordLog(sb, "获取 ISenparcEntities 对象成功");
                 RecordLog(sb, "开始生成 SQL ");
                 var sql = senparcEntities.Database.GenerateCreateScript();
                 RecordLog(sb, "SQL 已生成：");
                 RecordLog(sb, $"============ SCF Database {SystemTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}  ============");
                 RecordLog(sb, "");
                 RecordLog(sb, sql);
                 RecordLog(sb, "");
                 RecordLog(sb, $"============ SCF Database {SystemTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}  ============");
                 result.Message = "SQL 已生成，请下载日志文件！";
             });
        }
    }
}
