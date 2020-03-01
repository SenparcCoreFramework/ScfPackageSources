using Senparc.Scf.Core.Enums;
using Senparc.Scf.XscfBase;
using Senparc.Xscf.Terminal.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Senparc.Xscf.Terminal
{
    [XscfRegister]
    public class Register : IXscfRegister
    {
        public Register()
        { }

        #region IRegister 接口

        public string Name => "Senparc.Xscf.Terminal";
        public string Uid => "600C608A-F99A-4B1B-A18E-8CE69BE8DA92";//必须确保全局唯一，生成后必须固定
        public string Version => "0.0.5";//必须填写版本号

        public string MenuName => "终端模块";
        public string Description => "此模块提供给开发者一个可以直接使用终端命令控制系统的模块！";

        /// <summary>
        /// 注册当前模块需要支持的功能模块
        /// </summary>
        public IList<Type> Functions => new[] { 
            typeof(Functions.Terminal),
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
