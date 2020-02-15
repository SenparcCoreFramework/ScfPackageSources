
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
        /// 执行程序
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        string Run(params object[] param);
    }
}
