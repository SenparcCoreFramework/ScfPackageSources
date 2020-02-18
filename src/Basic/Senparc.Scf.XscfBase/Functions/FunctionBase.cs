using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace Senparc.Scf.XscfBase
{
    public abstract class FunctionBase : IXscfFunction
    {
        /// <summary>
        /// 方法名称
        /// </summary>
        public abstract string Name { get; }
        /// <summary>
        /// 说明
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// FunctionParameter 类型
        /// </summary>
        public abstract Type FunctionParameterType { get; }

        public virtual IFunctionParameter GenerateParameterInstance()
        {
            var obj = FunctionParameterType.Assembly.CreateInstance(FunctionParameterType.FullName) as IFunctionParameter;
            return obj;
        }

        /// <summary>
        /// ServiceProvider 实例
        /// </summary>
        public virtual IServiceProvider ServiceProvider { get; set; }


        public FunctionBase(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        /// <summary>
        /// 执行程序
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public abstract string Run(IFunctionParameter param);

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="msg"></param>
        protected void RecordLog(StringBuilder sb, string msg)
        {
            sb.AppendLine($"[{SystemTime.Now.ToString()}]\t{msg}");
        }

        /// <summary>
        /// 获取所有参数的信息列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<FunctionParammeterInfo> GetFunctionParammeterInfo()
        {
            var props = FunctionParameterType.GetProperties();
            foreach (var prop in props)
            {
                var name = prop.Name;
                string title = null;
                string description = null;
                var isRequired = prop.GetCustomAttribute<RequiredAttribute>() != null;
                var descriptionAttr = prop.GetCustomAttribute<DescriptionAttribute>();
                if (descriptionAttr != null && descriptionAttr.Description != null)
                {
                    var descriptionAttrArr = descriptionAttr.Description.Split(new[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                    title = descriptionAttrArr[0];
                    if (descriptionAttrArr.Length > 1)
                    {
                        description = descriptionAttrArr[1];
                    }
                }
                yield return new FunctionParammeterInfo(name, title, description, isRequired);
            }
        }
    }
}
