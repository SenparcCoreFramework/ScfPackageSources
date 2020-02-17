using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace Senparc.Scf.XscfBase
{
    public abstract class FunctionBase<T> : IXscfFunction<T> where T : IFunctionParameter, new()
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
        public virtual Type FunctionParameterType => typeof(T);

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
        public abstract string Run(T param);

        protected void RecordLog(StringBuilder sb, string msg)
        {
            sb.AppendLine($"[{SystemTime.Now.ToString()}]\t{msg}");
        }

        public IEnumerable<FunctionParammeterInfo> GetFunctionParammeterInfo()
        {

            var props = FunctionParameterType.GetProperties(BindingFlags.Public);
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
