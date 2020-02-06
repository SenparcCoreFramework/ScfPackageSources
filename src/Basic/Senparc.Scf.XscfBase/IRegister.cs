using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Senparc.Scf.XscfBase
{
    public interface IRegister
    {
        /// <summary>
        /// 模块名称
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        string MenuName { get; }
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
