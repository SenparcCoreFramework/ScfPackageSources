using Senparc.Scf.Core.Enums;
using Senparc.Scf.XscfBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Senparc.Xscf.DatabaseToolkit
{
    [XscfRegister]
    public class Register : IXscfRegister
    {
        public Register()
        { }

        #region IRegister 接口

        public string Name => "Senparc.Xscf.DatabaseToolkit"
            ;
        public string Uid => "3019CCBE-0739-43D5-9DED-027A0B26745E";//必须确保全局唯一，生成后必须固定
        public string Version => "0.2.1";//必须填写版本号

        public string MenuName => "数据库工具包";
        public string Icon => "fa fa-database";
        public string Description => "为方便数据库操作提供的工具包。请完全了解本工具各项功能特点后再使用，所有数据库操作都有损坏数据的可能，修改数据库前务必注意数据备份！";

        /// <summary>
        /// 注册当前模块需要支持的功能模块
        /// </summary>
        public IList<Type> Functions => new[] { 
            typeof(Functions.BackupDatabase),
            typeof(Functions.ExportSQL),
            typeof(Functions.CheckUpdate),
            typeof(Functions.UpdateDatabase),
        };

        public virtual Task InstallOrUpdateAsync(InstallOrUpdate installOrUpdate)
        {
            return Task.CompletedTask;
        }

        public virtual async Task UninstallAsync(Func<Task> unsinstallFunc)
        {
            await unsinstallFunc().ConfigureAwait(false);
        }

        #endregion
    }
}
