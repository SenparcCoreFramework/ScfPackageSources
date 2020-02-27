using Senparc.Scf.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Senparc.Scf.XscfBase
{
    public interface IXscfRegister
    {
        /// <summary>
        /// 模块名称，要求全局唯一
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 编号，要求全局唯一
        /// </summary>
        string Uid { get; }
        /// <summary>
        /// 版本号
        /// </summary>
        string Version { get; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        string MenuName { get; }
        /// <summary>
        /// 说明
        /// </summary>
        string Description { get; }
        /// <summary>
        /// 注册方法，注册的顺序决定了界面中排列的顺序
        /// </summary>
        IList<Type> Functions { get; }
        /// <summary>
        /// 安装代码
        /// </summary>
        Task InstallOrUpdateAsync(InstallOrUpdate installOrUpdate);
        /// <summary>
        /// 卸载代码
        /// </summary>
        Task UninstallAsync(Func<Task> unsinstallFunc);
    }
}
