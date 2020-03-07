using Microsoft.Extensions.DependencyInjection;
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
        /// Icon图标
        /// </summary>
        string Icon { get; }
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

        /// <summary>
        /// 获取首页Url
        /// <para>仅限实现了 IAreaRegister 接口之后的 Register，否则将返回 null</para>
        /// </summary>
        /// <returns></returns>
        string GetAreaHomeUrl();

        /// <summary>
        /// 获取 Area 其他页面的 URL
        /// </summary>
        /// <param name="path">URL 路径（不带 uid 参数）</param>
        /// <returns></returns>
        string GetAreaUrl(string path);

        /// <summary>
        /// 在 ConfigureServices 启动时注册当前模块
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        /// <returns></returns>
        IServiceCollection AddXscfModule(IServiceCollection services);
    }
}
