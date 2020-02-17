
using System;
using System.Collections.Generic;
using System.Text;

namespace Senparc.Scf.XscfBase
{
    /// <summary>
    /// 扩展方法接口
    /// </summary>
    public interface IXscfFunction<T> where T : IFunctionParameter
    {
        string Name { get; }
        string Description { get; }

        Type FunctionParameterType { get; }
        /// <summary>
        /// ServiceProvider 实例
        /// </summary>
        IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        /// 执行程序
        /// </summary>
        /// <param name="param">参数</param>
        /// <returns></returns>
        string Run(T param);
    }
}
