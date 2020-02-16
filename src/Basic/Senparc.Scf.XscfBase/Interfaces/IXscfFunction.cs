
using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Scf.XscfBase
{
    /// <summary>
    /// 扩展方法接口
    /// </summary>
    public interface IXscfFunction
    {
        /// <summary>
        /// 方法参数定义
        /// </summary>
        IList<FunctionParam> FunctionParams { get; }

        /// <summary>
        /// ServiceProvider 实例
        /// </summary>
        IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        /// 执行程序
        /// </summary>
        /// <param name="param">参数</param>
        /// <returns></returns>
        string Run(params object[] param);
    }
}
