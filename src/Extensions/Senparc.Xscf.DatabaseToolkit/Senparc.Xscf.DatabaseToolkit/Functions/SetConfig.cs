using Microsoft.EntityFrameworkCore;
using Senparc.Scf.Core.Models;
using Senparc.Scf.XscfBase;
using Senparc.Scf.XscfBase.Functions;
using System;
using Microsoft.Extensions.DependencyInjection;
using Senparc.Scf.Service;
using System.Threading.Tasks;

namespace Senparc.Xscf.DatabaseToolkit.Functions
{
    public class SetConfig : FunctionBase
    {
        public class SetConfig_Parameters : FunctionParameterLoadDataBase, IFunctionParameter
        {
            public int BackupCycleMinutes { get; set; }
            public string BackupPath { get; set; }

            public override async Task LoadData(IServiceProvider serviceProvider)
            {
                var configService = serviceProvider.GetService<ServiceBase<DbConfig>>();
                var config = await configService.GetObjectAsync(z => true);
                if (config != null)
                {
                    BackupCycleMinutes = config.BackupCycleMinutes;
                    BackupPath = config.BackuPath;
                }
            }
        }

        //注意：Name 必须在单个 Xscf 模块中唯一！
        public override string Name => "设置参数";

        public override string Description => "设置备份间隔时间、备份文件路径等参数";

        public override Type FunctionParameterType => typeof(SetConfig_Parameters);

        public SetConfig(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        /// <summary>
        /// 运行
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override FunctionResult Run(IFunctionParameter param)
        {
            return FunctionHelper.RunFunction<SetConfig_Parameters>(param, (typeParam, sb, result) =>
             {
                 RecordLog(sb, "开始获取 ISenparcEntities 对象");
                 var senparcEntities = ServiceProvider.GetService(typeof(ISenparcEntities)) as SenparcEntitiesBase;
                 RecordLog(sb, "获取 ISenparcEntities 对象成功");

                 var configService = base.ServiceProvider.GetService<ServiceBase<DbConfig>>();
                 var config = configService.GetObject(z => true);
                 if (config == null)
                 {
                     config = new DbConfig(typeParam.BackupCycleMinutes, typeParam.BackupPath);
                 }
                 else
                 {
                     configService.Mapper.Map(typeParam, config);
                 }
                 configService.SaveObject(config);

                 result.Message = "设置已保存！";
             });
        }
    }
}
