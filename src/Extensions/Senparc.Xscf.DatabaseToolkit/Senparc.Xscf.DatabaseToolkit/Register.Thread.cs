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
                task: async (app, threadInfo) =>
                {
                    try
                    {
                        SenparcTrace.SendCustomLog("执行调试", "DatabaseToolkit.Register.ThreadConfig");
                        threadInfo.RecordStory("开始检测并备份");
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
                                threadInfo.RecordStory("完成备份设置数据载入");
                            }
                            else
                            {
                                threadInfo.RecordStory("不需要备份，或没有设置，已忽略本次备份计划");
                                return;//不需要备份，或没有设置，返回
                            }
                        }
                        catch (Exception ex)
                        {
                            threadInfo.RecordStory(@$"遇到异常，可能未配置数据库，已忽略本次备份计划。如需启动，请更新此模块到最新版本。
异常信息：{ex.Message}
{ex.StackTrace}");
                            return;//可能没有配置数据库，返回
                        }

                        //执行备份方法
                        var result = backupDatabase.Run(backupParam);
                        if (!result.Success)
                        {
                            threadInfo.RecordStory("执行备份发生异常：" + result.Message);
                            throw new Exception("执行备份发生异常");
                        }
                        threadInfo.RecordStory("完成数据库自动备份：" + result.Message);
                        SenparcTrace.SendCustomLog("完成数据库自动备份", backupParam.Path);
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        threadInfo.RecordStory("检测并备份结束");
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
