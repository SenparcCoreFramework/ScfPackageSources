using Senparc.Scf.XscfBase.Functions;
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
        /// <para>注意：Name 必须在单个 Xscf 模块中唯一！</para>
        /// </summary>
        public abstract string Name { get; }

        //TODO:检查 name 冲突的情况

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
        public abstract FunctionResult Run(IFunctionParameter param);

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="msg"></param>
        protected void RecordLog(StringBuilder sb, string msg)
        {
            FunctionHelper.RecordLog(sb, msg);
        }

        /// <summary>
        /// 获取所有参数的信息列表
        /// </summary>
        /// <param name="tryLoadData">是否尝试载入数据（参数必须实现 IFunctionParameterLoadDataBase 接口）</param>
        /// <returns></returns>
        public IEnumerable<FunctionParameterInfo> GetFunctionParameterInfo(IServiceProvider serviceProvider, bool tryLoadData)
        {
            //TODO:提供自动载入数据的可选项

            var props = FunctionParameterType.GetProperties();
            ParameterType parameterType = ParameterType.Text;
            foreach (var prop in props)
            {
                List<string> selectionItems = null;
                var obj = GenerateParameterInstance();
                //判断是否存在选项
                if (prop.PropertyType.IsArray)
                {
                    var selection = prop.GetValue(obj, null);
                    if (selection == null)
                    {
                        continue;//此参数不加入
                    }

                    selectionItems = new List<string>();
                    parameterType = ParameterType.SingleSelection;//TODO:根据其他条件（如创建一个新的Attribute）判断多选
                    foreach (var item in (Array)selection)
                    {
                        selectionItems.Add(item.ToString());
                    }
                }

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
                var systemType = prop.PropertyType.Name;

                object value = null;
                if (tryLoadData && obj is IFunctionParameterLoadDataBase loadDataParam)
                {
                    loadDataParam.LoadData(serviceProvider);//载入参数
                    value = prop.GetValue(obj);
                }

                yield return new FunctionParameterInfo(name, title, description, isRequired, systemType, parameterType, selectionItems?.ToArray(), null);
            }
        }
    }
}
