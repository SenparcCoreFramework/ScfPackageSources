using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Senparc.Scf.XscfBase
{
    public interface IRegister
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
        /// 菜单名称
        /// </summary>
        string MenuName { get; }
        /// <summary>
        /// 说明
        /// </summary>
        string Description { get; }
        /// <summary>
        /// 安装代码
        /// </summary>
        void Install();
        /// <summary>
        /// 卸载代码
        /// </summary>
        void Uninstall();
    }
}
