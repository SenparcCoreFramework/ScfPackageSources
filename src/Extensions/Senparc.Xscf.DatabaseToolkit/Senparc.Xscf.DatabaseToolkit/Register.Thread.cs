using Senparc.CO2NET.Trace;
using Senparc.Scf.XscfBase;
using Senparc.Scf.XscfBase.Threads;
using Senparc.Xscf.DatabaseToolkit.Functions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Senparc.Xscf.DatabaseToolkit
{
    public partial class Register : IXscfThread
    {
        public void ThreadConfig(XscfThreadBuilder xscfThreadBuilder)
        {
            xscfThreadBuilder.AddThreadInfo(new Scf.XscfBase.Threads.ThreadInfo(
                name: "定时备份",
                intervalTime: TimeSpan.FromSeconds(10),
                task: async app =>
                {
                    try
                    {
                        var serviceProvider = app.ApplicationServices;
                        //初始化数据库备份方法
                        BackupDatabase backupDatabase = new BackupDatabase(serviceProvider);
                        //初始化参数
                        var backupParam = new BackupDatabase.BackupDatabase_Parameters();
                        var configParam = new SetConfig.SetConfig_Parameters();
                        try
                        {
                            //载入数据
                            await configParam.LoadData(serviceProvider);
                            if (configParam.BackupCycleMinutes > 0)
                            {
                                await backupParam.LoadData(serviceProvider);
                            }
                            else
                            {
                                return;//不需要备份，或没有设置，返回
                            }
                        }
                        catch (Exception)
                        {
                            return;//可能没有配置数据库，返回
                        }

                        //执行备份方法
                        var result = backupDatabase.Run(backupParam);
                        if (!result.Success)
                        {
                            throw new Exception("执行备份发生异常");
                        }

                        SenparcTrace.SendCustomLog("完成数据库自动备份", backupParam.Path);
                    }
                    catch
                    {
                        throw;
                    }

                },
                exceptionHandler: ex =>
                {
                    SenparcTrace.SendCustomLog("DatabaseToolkit", @$"{ex.Message}
{ex.StackTrace}
{ex.InnerException?.StackTrace}");
                    return Task.CompletedTask;
                }));
        }
    }
}
