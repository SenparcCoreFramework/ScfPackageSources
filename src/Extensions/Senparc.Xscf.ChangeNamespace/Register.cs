using Senparc.Scf.Core.Enums;
using Senparc.Scf.XscfBase;
using Senparc.Xscf.ChangeNamespace.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Senparc.Xscf.ChangeNamespace
{
    [XscfRegister]
    public class Register : IXscfRegister
    {
        public Register()
        { }

        #region IRegister 接口

        public string Name => "Senparc.Xscf.ChangeNamespace";
        public string Uid => "476A8F12-860D-4B18-B703-393BBDEFBD85";//必须确保全局唯一，生成后必须固定
        public string Version => "0.3.3";//必须填写版本号

        public string MenuName => "修改命名空间";
        public string Icon => "fa fa-terminal";
        public string Description => "此模块提供给开发者在安装完 SCF、发布产品之前，全局修改命名空间，请在生产环境中谨慎使用，此操作不可逆！必须做好提前备份！不建议在已经部署至生产环境并开始运行后使用此功能！";

        /// <summary>
        /// 注册当前模块需要支持的功能模块
        /// </summary>
        public IList<Type> Functions => new[] { 
            typeof(Functions.ChangeNamespace),
            typeof(Functions.RestoreNameSpace),
            typeof(Functions.DownloadSourceCode),
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
