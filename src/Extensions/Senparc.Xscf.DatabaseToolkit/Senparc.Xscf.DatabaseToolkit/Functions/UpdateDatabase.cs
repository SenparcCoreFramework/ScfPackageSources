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

    public class UpdateDatabase_Parameters : IFunctionParameter
    {

    }

    public class UpdateDatabase : FunctionBase
    {
        //注意：Name 必须在单个 Xscf 模块中唯一！
        public override string Name => "Merge EF Core";

        public override string Description => "使用 Entity Framework Core 的 Code First 模式对数据库进行更新，使数据库和当前运行版本匹配。";

        public override Type FunctionParameterType => typeof(UpdateDatabase_Parameters);

        public UpdateDatabase(IServiceProvider serviceProvider) : base(serviceProvider)
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
            var typeParam = param as UpdateDatabase_Parameters;
            StringBuilder sb = new StringBuilder();
            FunctionResult result = new FunctionResult()
            {
                Success = true
            };

            try
            {
                RecordLog(sb, "开始获取 ISenparcEntities 对象");
                ISenparcEntities senparcEntities = ServiceProvider.GetService(typeof(ISenparcEntities)) as ISenparcEntities;
                RecordLog(sb, "获取 ISenparcEntities 对象成功");
                RecordLog(sb, "开始重新标记 Merge 状态");
                senparcEntities.ResetMigrate();
                RecordLog(sb, "开始执行 Migrate()");
                senparcEntities.Migrate();
                RecordLog(sb, "执行 Migrate() 结束，操作完成");
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Exception = new XscfFunctionException(ex.Message, ex);

                RecordLog(sb, "发生错误：" + ex.Message);
                RecordLog(sb, ex.StackTrace.ToString());
            }

            result.Log = sb.ToString();
            return result;
        }
    }
}
