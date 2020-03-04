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
    public class Register : XscfRegisterBase, IXscfRegister
    {
        public Register()
        { }

        #region IRegister 接口

        public override string Name => "Senparc.Xscf.Terminal";
        public override string Uid => "600C608A-F99A-4B1B-A18E-8CE69BE8DA92";//必须确保全局唯一，生成后必须固定
        public override string Version => "0.1.3";//必须填写版本号

        public override string MenuName => "终端模块";
        public override string Icon => "fa fa-terminal";
        public override string Description => "此模块提供给开发者一个可以直接使用终端命令控制系统的模块！";

        /// <summary>
        /// 注册当前模块需要支持的功能模块
        /// </summary>
        public override IList<Type> Functions => new[] { 
            typeof(Functions.Terminal),
        };

        public override Task InstallOrUpdateAsync(InstallOrUpdate installOrUpdate)
        {
            return Task.CompletedTask;
        }

        public override async Task UninstallAsync(Func<Task> unsinstallFunc)
        {
            await unsinstallFunc().ConfigureAwait(false);
        }

        #endregion
    }
}
